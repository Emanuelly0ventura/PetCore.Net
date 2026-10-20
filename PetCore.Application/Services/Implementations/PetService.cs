using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Interfaces;

namespace PetCore.Application.Services.Implementations;

public class PetService(IPetRepository petRepository,
    ITutorRepository tutorRepository) : IPetService
{
    public IReadOnlyCollection<PetResponse> FetchAll(Guid? tutorId = null)
    {
        var pets = petRepository.FetchAll();
        if (tutorId.HasValue) pets = pets.Where(p => p.Tutores.Any(t => t.Id == tutorId.Value)).ToList();
        return pets
            .Select(pet => new PetResponse(pet))
            .ToList();
    }
    
    public IReadOnlyCollection<PetMenuResponse> FetchMenuAll()
    {
        return petRepository.FetchMenuAll()
            .Select(pet => new PetMenuResponse(pet))
            .ToList();
    }
    
    public PetResponse? FetchById(Guid id)
    {
        var pet = petRepository.FetchById(id);
        
        return pet is null ? null : new PetResponse(pet);
    }
    
    public PetResponse Create(PetRequest petRequest)
    {
        var pet = petRequest.ToDomain();

        var tutorIds = petRequest.IdTutores ?? [];
        if (petRequest.TutorIdMobile.HasValue && !tutorIds.Contains(petRequest.TutorIdMobile.Value)) tutorIds.Add(petRequest.TutorIdMobile.Value);
        if (tutorIds.Count == 0) throw new ArgumentException("Informe o tutorId do pet.");
        var tutores = tutorRepository.FetchAllById(tutorIds);
        if (tutores.Count != tutorIds.Distinct().Count()) throw new KeyNotFoundException("Um ou mais tutores informados não existem.");

        pet.Tutores = tutores.ToList();

        petRepository.Create(pet);
        petRepository.SaveChanges();

        return new PetResponse(pet);
    }
    
    public PetResponse? Patch(Guid id, PetDadosRequest petRequest)
    {
        var pet = petRepository.FetchById(id);

        if (pet is null)
            return null;
        
        var changed = false;

        if (petRequest.Nome is not null && petRequest.Especie is not null && petRequest.Raca is not null && petRequest.Pelagem is not null && petRequest.Porte is not null && petRequest.DataNasc.HasValue && petRequest.Sexo.HasValue)
        {
            pet.UpdateDados(petRequest.Nome, petRequest.Especie, petRequest.Raca, petRequest.DataNasc.Value,
                petRequest.Pelagem, petRequest.Porte, petRequest.Sexo.Value,
                petRequest.ObitoInformado ?? pet.ObitoInformado, petRequest.ComedouroStatus ?? pet.ComedouroStatus);
            changed = true;
        }
        else
        {
            if (petRequest.ObitoInformado.HasValue) { pet.UpdateObitoInformado(petRequest.ObitoInformado.Value); changed = true; }
            if (!string.IsNullOrWhiteSpace(petRequest.ComedouroStatus)) { pet.UpdateComedouroStatus(petRequest.ComedouroStatus); changed = true; }
        }
        
        if (petRequest.Status != null)
        {
            pet.UpdateStatus(petRequest.Status.Value);
            changed = true;
        }
        
        if (!string.IsNullOrWhiteSpace(petRequest.UrlImg))
        {
            pet.updateUrlImg(petRequest.UrlImg);
            changed = true;
        }

        
        if (changed)
        {
            petRepository.Patch(pet);
            petRepository.SaveChanges();
        }
        
        return new PetResponse(pet);
    }
    
    public bool Delete(Guid id)
    {
        var pet = petRepository.FetchById(id);

        if (pet is null)
            return false;

        petRepository.Delete(pet);
        petRepository.SaveChanges();
        return true;
    }
}
