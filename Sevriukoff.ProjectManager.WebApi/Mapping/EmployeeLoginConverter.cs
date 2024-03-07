using System.Text.Json;
using System.Text.Json.Serialization;
using Sevriukoff.ProjectManager.WebApi.ViewModels.Employee;

namespace Sevriukoff.ProjectManager.WebApi.Mapping;

public class EmployeeLoginConverter : JsonConverter<LoginViewModel>
{
    public override LoginViewModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions? options)
    {
        using (var doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;
            
            if (root.TryGetProperty("email", out _))
                return JsonSerializer.Deserialize<LoginByEmailViewModel>(root.GetRawText(), options);

            if (root.TryGetProperty("username", out _))
                return JsonSerializer.Deserialize<LoginByNameViewModel>(root.GetRawText(), options);

            if (root.TryGetProperty("id", out _))
                return JsonSerializer.Deserialize<LoginByIdViewModel>(root.GetRawText(), options);

            return JsonSerializer.Deserialize<LoginViewModel>(root.GetRawText(), options);
        }
    }

    public override void Write(Utf8JsonWriter writer, LoginViewModel value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}