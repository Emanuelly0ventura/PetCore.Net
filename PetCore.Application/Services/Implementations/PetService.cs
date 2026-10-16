using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Interfaces;

namespace PetCore.Application.Services.Implementations;

public class PetService(IPetRepository petRepository,
    ITutorRepository tutorRepository) : IPetService
{
    public IReadOnlyCollection<PetResponse> FetchAll()
    {
        return petRepository.FetchAll()
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

        var tutores = tutorRepository
            .FetchAllById(petRequest.IdTutores ?? []);

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
        
        if (petRequest.Status != null)
        {
            pet.UpdateStatus(petRequest.Status);
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
