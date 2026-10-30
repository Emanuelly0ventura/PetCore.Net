using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Interfaces;
using PetCore.Application.Security;

namespace PetCore.Application.Services.Implementations;

public class MedicoService(IMedicoRepository medicoRepository, IPasswordHashService passwordHasher) : IMedicoService
{
        public IReadOnlyCollection<MedicoResponse> FetchAll()
    {
        return medicoRepository.FetchAll()
            .Select(med => new MedicoResponse(med))
            .ToList();
    }
    
    public PagedData<MedicoResponse> FetchPage(PageQuery query)
    {
        var page = medicoRepository.FetchPage(query);
        return new PagedData<MedicoResponse>(page.Items.Select(item => new MedicoResponse(item)).ToList(), page.Page, page.PageSize, page.TotalItems, page.TotalPages);
    }

    public MedicoResponse? FetchById(Guid id)
    {
        var med = medicoRepository.FetchById(id);
        
        return med is null ? null : new MedicoResponse(med);
    }
    
    public MedicoResponse Create(MedicoRequest medicoRequest)
    {
        var med = medicoRequest.ToDomain();
        med.UpdateSenha(passwordHasher.Hash(med.Senha));
        medicoRepository.Create(med);
        medicoRepository.SaveChanges();

        return new MedicoResponse(med);
    }
    
    public MedicoResponse? Patch(Guid id, UserDadosRequest userRequest)
    {
        var changed = false;
        var med = medicoRepository.FetchById(id);

        if (med is null)
            return null;

        if (!string.IsNullOrWhiteSpace(userRequest.Nome)) { med.Nome = userRequest.Nome.Trim(); changed = true; }
        var nascimento = userRequest.ParseNascimento();
        if (nascimento.HasValue)
        {
            if (nascimento.Value > DateOnly.FromDateTime(DateTime.Now.AddYears(-18))) throw new ArgumentException("Médico deve ser maior de 18 anos.");
            med.DataNascimento = nascimento.Value; changed = true;
        }
        var genero = userRequest.ParseGenero();
        if (genero.HasValue) { med.Sexo = genero.Value; changed = true; }
        if (!string.IsNullOrWhiteSpace(userRequest.Especializacao)) { med.UpdateEspecialidade(userRequest.Especializacao); changed = true; }
        if (userRequest.Clinica is not null || userRequest.NomeClinica is not null || userRequest.Cnpj is not null || userRequest.Cep is not null || userRequest.Complemento is not null)
        {
            med.UpdateDadosClinica(userRequest.Clinica, userRequest.NomeClinica, userRequest.Cnpj, userRequest.Cep, userRequest.Complemento);
            changed = true;
        }
        
        if (!string.IsNullOrWhiteSpace(userRequest.Email))
        {
            med.UpdateEmail(userRequest.Email);
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(userRequest.Telefone))
        {
            med.UpdateTelefone(userRequest.Telefone);
            changed = true;
        }
        
        if (!string.IsNullOrWhiteSpace(userRequest.Senha))
        {
            med.UpdateSenha(passwordHasher.Hash(userRequest.Senha));
            changed = true;
        }
        
        if (!string.IsNullOrWhiteSpace(userRequest.UrlImg))
        {
            med.UpdateUrlImg(userRequest.UrlImg);
            changed = true;
        }
        
        if (changed)
        {
            medicoRepository.Patch(med);
            medicoRepository.SaveChanges();
        }
        
        return new MedicoResponse(med);
    }
    
    public bool Delete(Guid id)
    {
        var rel = medicoRepository.FetchById(id);

        if (rel is null)
            return false;

        medicoRepository.Delete(rel);
        medicoRepository.SaveChanges();
        return true;
    }

    public MedicoResponse? FetchByEmail(string email, string senha)
    {
        var medico = medicoRepository.FetchByEmail(email);
        if (medico is null || !passwordHasher.Verify(senha, medico.Senha))
            return null;

        if (!passwordHasher.IsHash(medico.Senha))
        {
            medico.UpdateSenha(passwordHasher.Hash(senha));
            medicoRepository.Patch(medico);
            medicoRepository.SaveChanges();
        }

        return new MedicoResponse(medico);
    }
}
