using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Interfaces;

namespace PetCore.Application.Services.Implementations;

public class EnderecoService(IEnderecoRepository enderecoRepository) : IEnderecoService
{
    public IReadOnlyCollection<EnderecoResponse> FetchAll()
    {
        return enderecoRepository.FetchAll()
            .Select(end => new EnderecoResponse(end))
            .ToList();
    }
    
    public EnderecoResponse? FetchById(Guid id)
    {
        var end = enderecoRepository.FetchById(id);
        
        return end is null ? null : new EnderecoResponse(end);
    }
    
    public EnderecoResponse Create(EnderecoRequest enderecoRequest)
    {
        var content = enderecoRequest.ToDomain();
        enderecoRepository.Create(content);
        enderecoRepository.SaveChanges();

        return new EnderecoResponse(content);
    }
    
    public EnderecoResponse? Patch(Guid id, EnderecoRequest enderecoRequest)
    {
        var changed = false;
        var end = enderecoRepository.FetchById(id);

        if (end is null)
            return null;
        
        if (!string.IsNullOrWhiteSpace(enderecoRequest.Cep))
        {
            end.UpdateCep(enderecoRequest.Cep);
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(enderecoRequest.Complemento))
        {
            end.UpdateComplemento(enderecoRequest.Complemento);
            changed = true;
        }
        
        if (changed)
        {
            enderecoRepository.Patch(end);
            enderecoRepository.SaveChanges();
        }
        
        return new EnderecoResponse(end);
    }
    
    public bool Delete(Guid id)
    {
        var end = enderecoRepository.FetchById(id);

        if (end is null)
            return false;

        enderecoRepository.Delete(end);
        enderecoRepository.SaveChanges();
        return true;
    }
}