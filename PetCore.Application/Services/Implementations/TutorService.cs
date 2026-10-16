using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Interfaces;

namespace PetCore.Application.Services.Implementations;

public class TutorService(ITutorRepository tutorRepository) : ITutorService
{
    public IReadOnlyCollection<TutorResponse> FetchAll()
    {
        return tutorRepository.FetchAll()
            .Select(tut => new TutorResponse(tut))
            .ToList();
    }
    
    public TutorResponse? FetchById(Guid id)
    {
        var tut = tutorRepository.FetchById(id);
        
        return tut is null ? null : new TutorResponse(tut);
    }
    
    public TutorResponse Create(TutorRequest tutorRequest)
    {
        var tut = tutorRequest.ToDomain();
        tutorRepository.Create(tut);
        tutorRepository.SaveChanges();

        return new TutorResponse(tut);
    }
    
    public TutorResponse? Patch(Guid id, UserDadosRequest userRequest)
    {
        var changed = false;
        var tut = tutorRepository.FetchById(id);

        if (tut is null)
            return null;
        
        if (!string.IsNullOrWhiteSpace(userRequest.Email))
        {
            tut.UpdateEmail(userRequest.Email);
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(userRequest.Telefone))
        {
            tut.UpdateTelefone(userRequest.Telefone);
            changed = true;
        }
        
        if (!string.IsNullOrWhiteSpace(userRequest.Senha))
        {
            tut.UpdateSenha(userRequest.Senha);
            changed = true;
        }
        
        if (!string.IsNullOrWhiteSpace(userRequest.UrlImg))
        {
            tut.UpdateUrlImg(userRequest.UrlImg);
            changed = true;
        }
        
        if (changed)
        {
            tutorRepository.Patch(tut);
            tutorRepository.SaveChanges();
        }
        
        return new TutorResponse(tut);
    }
    
    public bool Delete(Guid id)
    {
        var rel = tutorRepository.FetchById(id);

        if (rel is null)
            return false;

        tutorRepository.Delete(rel);
        tutorRepository.SaveChanges();
        return true;
    }

    public TutorResponse? FetchByEmail(string email, string senha)
    {
        var tutor = tutorRepository.FetchAll()
            .FirstOrDefault(t => t.Email == email && t.Senha == senha);

        return tutor is null ? null : new TutorResponse(tutor);
    }
}