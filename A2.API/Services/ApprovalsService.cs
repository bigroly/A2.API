using A2.API.Models.Requests;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.API.Services
{
  public interface IApprovalsService
  {
    Task<bool> AddUpdateApproval(Approval item);
    Task<List<Approval>> GetApprovalsForUser(string userEmail);
  }

  public class ApprovalsService: IApprovalsService
  {
    private IUtilityService _utils;
    private readonly IAmazonDynamoDB _dynamoDb;
    private IConfiguration _config;

    public ApprovalsService(IUtilityService utils,
                               IAmazonDynamoDB dynamoDb,
                               IConfiguration config)
    {
      _utils = utils;
      _dynamoDb = dynamoDb;
      _config = config;
    }

    public async Task<bool> AddUpdateApproval(Approval item)
    {
      PutItemRequest putRequest = new PutItemRequest()
      {
        TableName = _config.GetValue<string>("DynamoDbTables:ApprovalsTableName"),
        Item = _utils.ToDynamoAttributeValueDictionary<Approval>(item)
      };

      await _dynamoDb.PutItemAsync(putRequest);
      return true;
    }

    public async Task<List<Approval>> GetApprovalsForUser(string userEmail)
    {
      List<Approval> allApprovals = new List<Approval>();

      QueryRequest query = new QueryRequest()
      {
        TableName = _config.GetValue<string>("DynamoDbTables:ApprovalsTableName"),
        ReturnConsumedCapacity = "TOTAL",
        KeyConditionExpression = "Approver = :v_Approver",
        ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
        {
            {
                ":v_Approver",
                new AttributeValue
                {
                    S = userEmail
                }
            }
        }
      };

      var queryResults = await _dynamoDb.QueryAsync(query);
      foreach (var item in queryResults.Items)
      {
        allApprovals.Add(_utils.ToObjectFromDynamoResult<Approval>(item));
      }
      return allApprovals;
    }
  }
}
