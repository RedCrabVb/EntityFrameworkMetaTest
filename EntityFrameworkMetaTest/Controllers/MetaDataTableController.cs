using EntityFrameworkMetaTest.Data;
using EntityFrameworkMetaTest.Model;
using EntityFrameworkMetaTest.Model.Meta;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection.Emit;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Runtime.CompilerServices;
using Npgsql;

namespace EntityFrameworkMetaTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetaDataTableController : ControllerBase
    {
        private readonly EntityFrameworkMetaTestContext _context;

        public MetaDataTableController(EntityFrameworkMetaTestContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("GetMeta")]
        public IEnumerable<MetaTableModel> Get()
        {
            return _context.MetaTable.ToList();
        }

        [HttpGet]
        [Route("GetData")]
        public object GetData(string extid)
        {
            var meta = _context.MetaTable.Where(m => m.ExtId == extid).FirstOrDefault() ?? throw new ArgumentNullException();
            var metaData = JsonConvert.DeserializeObject<MetaTable>(meta.JsonText);

            using var connection = new NpgsqlConnection("Host=localhost;Port=32768;Database=postgres;Username=postgres;Password=postgrespw");
            connection.Open();

            using var command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = $"select * from {metaData.TableName}";

            using var reader = command.ExecuteReader();

            var result = new List<Dictionary<string, object>>();

            while (reader.Read())
            {
                var dictionary = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    object value = reader.GetValue(i);

                    dictionary[columnName] = value;
                }

                result.Add(dictionary);
            }

            return result;
        }


        //TypeBuilder builder = CreateTypeBuilder(
        //        "MyDynamicAssembly", "MyModule", "MyType");

        //var mapping = new Dictionary<string, Type>()
        //{
        //    {"number", typeof(int) },
        //    {"string", typeof(string) },
        //    {"boolean", typeof(bool) },
        //};

        //foreach (var i in metaData.Columns)
        //{
        //    CreateAutoImplementedProperty(builder, i.Field, mapping[i.Type]);
        //}

        //Type resultType = builder.CreateType();

        //ModelConfigurationBuilder.DefaultTypeMapping(resultType, builder234 =>
        //{
        //    builder234.HasColumnType("f");
        //    // Configure the type mapping here
        //    //builder234.ToTable("MyTable"); // Specify the table name for the type
        //    //builder234.Property("PropertyName").HasColumnName("Column"); // Map a property to a specific column
        //                                                              // Add more configuration as needed
        //});

        //var fd = typeof(ModelConfigurationBuilder)
        //    .GetMethods().Where(m => m.Name == "DefaultTypeMapping" && m.GetParameters().Count() == 0).FirstOrDefault()
        //    .MakeGenericMethod(resultType).Invoke(null, new object[] {  });


        //var method = typeof(RelationalDatabaseFacadeExtensions)
        //    .GetMethod("SqlQuery");
        //var result = method.MakeGenericMethod(resultType)
        //    .Invoke(null, new object[] { _context.Database, FormattableStringFactory.Create($"SELECT * FROM {metaData.TableName}") });


        //public static TypeBuilder CreateTypeBuilder(
        //    string assemblyName, string moduleName, string typeName)
        //{
        //    var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.Run);
        //    ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleName);
        //    TypeBuilder tb = moduleBuilder.DefineType(typeName,
        //            TypeAttributes.Public |
        //            TypeAttributes.Class |
        //            TypeAttributes.AutoClass |
        //            TypeAttributes.AnsiClass |
        //            TypeAttributes.BeforeFieldInit |
        //            TypeAttributes.AutoLayout,
        //            null);
        //    return tb;

        //}

        //public static void CreateAutoImplementedProperty(
        //    TypeBuilder builder, string propertyName, Type propertyType)
        //{
        //    const string PrivateFieldPrefix = "m_";
        //    const string GetterPrefix = "get_";
        //    const string SetterPrefix = "set_";

        //    // Generate the field.
        //    FieldBuilder fieldBuilder = builder.DefineField(
        //        string.Concat(PrivateFieldPrefix, propertyName),
        //                      propertyType, FieldAttributes.Private);

        //    // Generate the property
        //    PropertyBuilder propertyBuilder = builder.DefineProperty(
        //        propertyName, PropertyAttributes.HasDefault, propertyType, null);

        //    // Property getter and setter attributes.
        //    MethodAttributes propertyMethodAttributes =
        //        MethodAttributes.Public | MethodAttributes.SpecialName |
        //        MethodAttributes.HideBySig;

        //    // Define the getter method.
        //    MethodBuilder getterMethod = builder.DefineMethod(
        //        string.Concat(GetterPrefix, propertyName),
        //        propertyMethodAttributes, propertyType, Type.EmptyTypes);

        //    // Emit the IL code.
        //    // ldarg.0
        //    // ldfld,_field
        //    // ret
        //    ILGenerator getterILCode = getterMethod.GetILGenerator();
        //    getterILCode.Emit(OpCodes.Ldarg_0);
        //    getterILCode.Emit(OpCodes.Ldfld, fieldBuilder);
        //    getterILCode.Emit(OpCodes.Ret);

        //    // Define the setter method.
        //    MethodBuilder setterMethod = builder.DefineMethod(
        //        string.Concat(SetterPrefix, propertyName),
        //        propertyMethodAttributes, null, new Type[] { propertyType });

        //    // Emit the IL code.
        //    // ldarg.0
        //    // ldarg.1
        //    // stfld,_field
        //    // ret
        //    ILGenerator setterILCode = setterMethod.GetILGenerator();
        //    setterILCode.Emit(OpCodes.Ldarg_0);
        //    setterILCode.Emit(OpCodes.Ldarg_1);
        //    setterILCode.Emit(OpCodes.Stfld, fieldBuilder);
        //    setterILCode.Emit(OpCodes.Ret);

        //    propertyBuilder.SetGetMethod(getterMethod);
        //    propertyBuilder.SetSetMethod(setterMethod);
        //}
    }
}
