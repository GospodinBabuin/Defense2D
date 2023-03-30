using System.Runtime.Serialization;
using UnityEngine;

public class BuildingsSerializationSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        var building = (Buildings) obj;
        info.AddValue("Health", building.Health);
        info.AddValue("PositionX", building.transform.position.x);
        info.AddValue("PositionY", building.transform.position.y);
        info.AddValue("Name", building.buildingType);
        info.AddValue("Lvl", building.BuildingLevel);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        var building = (Buildings)obj;
        building.Health = (int)info.GetValue("Health", typeof(int));
        building.PositionX = (float)info.GetValue("PositionX", typeof(float));
        building.PositionY = (float)info.GetValue("PositionY", typeof(float));
        building.buildingType = (string)info.GetValue("Name", typeof(string));
        building.BuildingLevel = (int)info.GetValue("Lvl", typeof(int));
        obj = building;
        return obj;
    }
}
