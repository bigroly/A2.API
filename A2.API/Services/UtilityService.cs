using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace A2.API.Services
{
  public interface IUtilityService
  {
    Dictionary<string, AttributeValue> ToDynamoAttributeValueDictionary<T>(T obj);
    T ToObjectFromDynamoResult<T>(Dictionary<string, AttributeValue> dynamoObj) where T : new();
  }
  public class UtilityService : IUtilityService
  {
    public Dictionary<string, AttributeValue> ToDynamoAttributeValueDictionary<T>(T obj)
    {

      Dictionary<string, AttributeValue> objectProps = new Dictionary<string, AttributeValue>();
      PropertyInfo[] props = obj.GetType().GetProperties();

      foreach (var prop in props)
      {
        AttributeValue propertyValue;
        switch (prop.PropertyType.Name.ToLower())
        {
          case "string":
            propertyValue = new AttributeValue()
            {
              S = (string)prop.GetValue(obj)
            };
            break;
          case "int":
            propertyValue = new AttributeValue()
            {
              N = prop.GetValue(obj).ToString()
            };
            break;
          case "int32":
            propertyValue = new AttributeValue()
            {
              N = prop.GetValue(obj).ToString()
            };
            break;
          case "int64":
            propertyValue = new AttributeValue()
            {
              N = prop.GetValue(obj).ToString()
            };
            break;
          case "long":
            propertyValue = new AttributeValue()
            {
              N = (string)prop.GetValue(obj)
            };
            break;
          case "float":
            propertyValue = new AttributeValue()
            {
              N = (string)prop.GetValue(obj)
            };
            break;
          case "bool":
            propertyValue = new AttributeValue()
            {
              BOOL = (bool)prop.GetValue(obj)
            };
            break;
          case "boolean":
            propertyValue = new AttributeValue()
            {
              BOOL = (bool)prop.GetValue(obj)
            };
            break;
          case "datetime":
            propertyValue = new AttributeValue()
            {
              S = ((DateTime)prop.GetValue(obj)).ToString("s", System.Globalization.CultureInfo.InvariantCulture)
            };
            break;
          default:
            propertyValue = new AttributeValue()
            {
              S = (string)prop.GetValue(obj)
            };
            break;
        }
        objectProps.Add(prop.Name, propertyValue);
      }

      return objectProps;
    }

    public T ToObjectFromDynamoResult<T>(Dictionary<string, AttributeValue> dynamoObj) where T : new()
    {
      var returnObj = new T();
      Dictionary<string, AttributeValue> objectProps = new Dictionary<string, AttributeValue>();

      PropertyInfo[] props = returnObj.GetType().GetProperties();

      foreach (var prop in props)
      {
        var dynamoObjValue = dynamoObj.Where(d => d.Key == prop.Name).FirstOrDefault().Value;

        switch (prop.PropertyType.Name.ToLower())
        {
          case "string":
            prop.SetValue(returnObj, dynamoObjValue.S);
            break;
          case "int":
            prop.SetValue(returnObj, Convert.ToInt32(dynamoObjValue.N));
            break;
          case "int32":
            prop.SetValue(returnObj, Convert.ToInt32(dynamoObjValue.N));
            break;
          case "int64":
            prop.SetValue(returnObj, Convert.ToInt64(dynamoObjValue.N));
            break;
          case "long":
            prop.SetValue(returnObj, Convert.ToInt64(dynamoObjValue.N));
            break;
          case "double":
            prop.SetValue(returnObj, Convert.ToDouble(dynamoObjValue.S));
            break;
          case "boolean":
            prop.SetValue(returnObj, dynamoObjValue.BOOL);
            break;
          case "datetime":
            prop.SetValue(returnObj, DateTime.Parse(dynamoObjValue.S));
            break;
        }
      }

      return returnObj;
    }
  }
}
