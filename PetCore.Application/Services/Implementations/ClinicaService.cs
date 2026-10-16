using PetCore.Application.DTOs;
using PetCore.Application.Interfaces;
using PetCore.Application.Services.Interfaces;

namespace PetCore.Application.Services.Implementations;

public class ClinicaService(IClinicaRepository clinicaRepository) : IClinicaService
{
    public IReadOnlyCollection<ClinicaResponse> FetchAll()
    {
        return clinicaRepository.FetchAll()
            .Select(clinica => new ClinicaResponse(clinica))
            .ToList();
    }
    
    public ClinicaResponse? FetchById(Guid id)
    {
        var clinica = clinicaRepository.FetchById(id);
        
        return clinica is null ? null : new ClinicaResponse(clinica);
    }
    
    public ClinicaResponse Create(ClinicaRequest clinicaRequest)
    {
        var content = clinicaRequest.ToDomain();
        clinicaRepository.Create(content);
        clinicaRepository.SaveChanges();

        return new ClinicaResponse(content);
    }
    
    public ClinicaResponse? Patch(Guid id, ClinicaDadosRequest clinicaRequest)
    {
        var changed = false;
        var clinica = clinicaRepository.FetchById(id);

        if (clinica is null)
            return null;
        
        if (!string.IsNullOrWhiteSpace(clinicaRequest.Nome))
        {
            clinica.UpdateNome(clinicaRequest.Nome);
            changed = true;
        }

        if (!(clinicaRequest.IdEndereco == Guid.Empty))
        {
            clinica.UpdateEndereco(clinicaRequest.IdEndereco);
            changed = true;
        }
        
        if (changed)
        {
            clinicaRepository.Patch(clinica);
            clinicaRepository.SaveChanges();
        }
        
        return new ClinicaResponse(clinica);
    }
    
    public bool Delete(Guid id)
    {
        var clinica = clinicaRepository.FetchById(id);

        if (clinica is null)
            return false;

        clinicaRepository.Delete(clinica);
        clinicaRepository.SaveChanges();
        return true;
    }
}