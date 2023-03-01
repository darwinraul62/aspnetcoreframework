using System;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.EntityFrameworkCore
{
    public static class ModelBuilderExtensions
    {
        public static void ToLowerCaseEntities(this ModelBuilder builder)
        {
            foreach(var entity in builder.Model.GetEntityTypes())
            {                              
                entity.SetTableName(entity.GetTableName().ToLower());

                // Replace column names            
                foreach(var property in entity.GetProperties())
                {                    
                    property.SetColumnName(property.Name.ToLower());
                }

                // foreach(var key in entity.GetKeys())
                // {
                //     key.Relational().Name = key.Relational().Name.ToSnakeCase();
                // }

                // foreach(var key in entity.GetForeignKeys())
                // {
                //     key.Relational().Name = key.Relational().Name.ToSnakeCase();
                // }

                // foreach(var index in entity.GetIndexes())
                // {
                //     index.Relational().Name = index.Relational().Name.ToSnakeCase();
                // }
            }

        }        
    }
}
