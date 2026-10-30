using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Interfaces;
using PetCore.Application.Security;

namespace PetCore.Application.Services.Implementations;

public class TutorService(ITutorRepository tutorRepository, IPasswordHashService passwordHasher) : ITutorService
{
    public IReadOnlyCollection<TutorResponse> FetchAll()
    {
        return tutorRepository.FetchAll()
            .Select(tut => new TutorResponse(tut))
            .ToList();
    }
    
    public PagedData<TutorResponse> FetchPage(PageQuery query)
    {
        var page = tutorRepository.FetchPage(query);
        return new PagedData<TutorResponse>(page.Items.Select(item => new TutorResponse(item)).ToList(), page.Page, page.PageSize, page.TotalItems, page.TotalPages);
    }

    public TutorResponse? FetchById(Guid id)
    {
        var tut = tutorRepository.FetchById(id);
        
        return tut is null ? null : new TutorResponse(tut);
    }
    
    public TutorResponse Create(TutorRequest tutorRequest)
    {
        var tut = tutorRequest.ToDomain();
        tut.UpdateSenha(passwordHasher.Hash(tut.Senha));
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

        if (!string.IsNullOrWhiteSpace(userRequest.Nome)) { tut.Nome = userRequest.Nome.Trim(); changed = true; }
        var nascimento = userRequest.ParseNascimento();
        if (nascimento.HasValue)
        {
            if (nascimento.Value > DateOnly.FromDateTime(DateTime.Now.AddYears(-18))) throw new ArgumentException("Tutor deve ser maior de 18 anos.");
            tut.DataNascimento = nascimento.Value; changed = true;
        }
        var genero = userRequest.ParseGenero();
        if (genero.HasValue) { tut.Sexo = genero.Value; changed = true; }
        
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
            tut.UpdateSenha(passwordHasher.Hash(userRequest.Senha));
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
        var tutor = tutorRepository.FetchByEmail(email);
        if (tutor is null || !passwordHasher.Verify(senha, tutor.Senha))
            return null;

        if (!passwordHasher.IsHash(tutor.Senha))
        {
            tutor.UpdateSenha(passwordHasher.Hash(senha));
            tutorRepository.Patch(tutor);
            tutorRepository.SaveChanges();
        }

        return new TutorResponse(tutor);
    }
}
