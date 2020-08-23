using A2.API.Models;
using A2.API.Models.Users;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.API.Services
{
  public interface IUserService
  {
    Task<List<User>> GetAllUsers();
    Task<User> GetUser(string userEmail);
    Task<bool> CreateUser(User user);
    Task<bool> UpdateUser(User user);
  }

  public class UserService : IUserService
  {
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly IAmazonCognitoIdentityProvider _cognito;
    private readonly IConfiguration _config;
    private IUtilityService _utils;

    public UserService(IAmazonDynamoDB dynamoDb,
                       IAmazonCognitoIdentityProvider cognito,
                       IConfiguration config,
                       IUtilityService utils)
    {
      _dynamoDb = dynamoDb;
      _cognito = cognito;
      _config = config;
      _utils = utils;
    }

    public async Task<List<User>> GetAllUsers()
    {
      List<User> allUsers = new List<User>();

      ScanRequest scanReq = new ScanRequest()
      {
        TableName = _config.GetValue<string>("DynamoDbTables:UserTableName")
      };

      var scanResults = await _dynamoDb.ScanAsync(scanReq);

      foreach (var item in scanResults.Items)
      {
        allUsers.Add(_utils.ToObjectFromDynamoResult<User>(item));
      }

      return allUsers;
    }

    public async Task<User> GetUser(string userEmail)
    {      

      QueryRequest query = new QueryRequest()
      {
        TableName = _config.GetValue<string>("DynamoDbTables:UserTableName"),
        ReturnConsumedCapacity = "TOTAL",
        KeyConditionExpression = "EmailAddress = :v_EmailAddress",
        ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
        {
            {
                ":v_EmailAddress",
                new AttributeValue
                {
                    S = userEmail
                }
            }
        }
      };

      var queryResults = await _dynamoDb.QueryAsync(query);

      if (!queryResults.Items.Any())
      {
        return null;
      }

      return _utils.ToObjectFromDynamoResult<User>(queryResults.Items.First());      
    }

    public async Task<bool> CreateUser(User user)
    {
      AdminCreateUserRequest cognitoCreateUserRequest = new AdminCreateUserRequest()
      {
        UserPoolId = _config.GetValue<string>("Cognito:PoolId"),
        Username = user.EmailAddress,
        DesiredDeliveryMediums = new List<string> { "EMAIL" }
      };

      await _cognito.AdminCreateUserAsync(cognitoCreateUserRequest);
      await CognitoPutUser(user);

      return true;
    }

    public async Task<bool> UpdateUser(User user)
    {
      return await CognitoPutUser(user);
    }

    private async Task<bool> CognitoPutUser(User user)
    {
      PutItemRequest putRequest = new PutItemRequest()
      {
        TableName = _config.GetValue<string>("DynamoDbTables:UserTableName"),
        Item = _utils.ToDynamoAttributeValueDictionary<User>(user)
      };

      await _dynamoDb.PutItemAsync(putRequest);
      return true;
    }

  }
}
