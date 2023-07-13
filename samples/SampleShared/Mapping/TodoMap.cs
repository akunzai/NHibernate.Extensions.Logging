using FluentNHibernate.Mapping;
using NHibernate.Type;
using SampleShared.Domain;

namespace SampleShared.Mapping;

public class TodoMap : ClassMap<Todo>
{
    public TodoMap()
    {
        Table("Todos");
        Id(x => x.Id).GeneratedBy.GuidComb();
        Map(x => x.Title).Not.Nullable();
        Map(x => x.Completed).Not.Nullable();
        Map(x => x.Created).CustomType<LocalDateTimeType>();
    }
}