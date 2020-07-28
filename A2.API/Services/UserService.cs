﻿using A2.API.Models;
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
  }

  public class UserService : IUserService
  {
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly IConfiguration _config;
    private IUtilityService _utils;

    public UserService(IAmazonDynamoDB dynamoDb,
                       IConfiguration config,
                       IUtilityService utils)
    {
      _dynamoDb = dynamoDb;
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

    public async Task<bool> CreateUser(User user)
    {
      var putRequest = new PutItemRequest()
      {
        TableName = _config.GetValue<string>("DynamoDbTables:UserTableName"),
        Item = _utils.ToDynamoAttributeValueDictionary<User>(user)
      };

      await _dynamoDb.PutItemAsync(putRequest);
      return true;
    }

  }
}
