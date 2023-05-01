namespace VehicleGenius.Api.Services.Mappers;

public interface IMapperService<Model, Dto>
{
  Model MapToModel(Dto dto);
  Dto MapToDto(Model model);
}
