using System.Runtime.Serialization;

public class SoldersSerializationSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        var solder = (AlliesSolders)obj;
        info.AddValue("hp", solder.Health);
        info.AddValue("name", solder.name);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        var solder = (AlliesSolders)obj;
        solder.Health = (int)info.GetValue("hp", typeof(int));
        solder.Name = (string)info.GetValue("name", typeof(string));
        obj = solder;
        return obj;
    }
}
