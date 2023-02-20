using Mapster;
using ToDoListApp.Dto;

namespace ToDoListApp
{
    public class ModelMapping : IRegister
    {
        void IRegister.Register(TypeAdapterConfig config)
        {
            config.ForType<Todo, TodoDto>()
                .Map(dest => dest.Date, src => src.Updated.ToLongDateString())
                .Map(dest => dest.Time, src => src.Updated.ToShortTimeString());
        }
    }
}
