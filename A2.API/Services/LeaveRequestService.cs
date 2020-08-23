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
  public interface ILeaveRequestService
  {
    Task<List<LeaveRequest>> GetLeaveRequests(string requestorEmail);
    Task<LeaveRequest> AddUpdateLeaveRequest(LeaveRequest request);
  }

  public class LeaveRequestService: ILeaveRequestService
  {
    private IUtilityService _utils;
    private readonly IAmazonDynamoDB _dynamoDb;
    private IConfiguration _config;

    public LeaveRequestService(IUtilityService utils,
                               IAmazonDynamoDB dynamoDb,
                               IConfiguration config)
    {
      _utils = utils;
      _dynamoDb = dynamoDb;
      _config = config;
    }

    public async Task<List<LeaveRequest>> GetLeaveRequests(string requestorEmail)
    {
      List<LeaveRequest> allRequests = new List<LeaveRequest>();

      QueryRequest query = new QueryRequest()
      {
        TableName = _config.GetValue<string>("DynamoDbTables:LeaveRequestsTableName"),
        ReturnConsumedCapacity = "TOTAL",
        KeyConditionExpression = "Requestor = :v_Requestor",
        ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
        {
            {
                ":v_Requestor",
                new AttributeValue
                {
                    S = requestorEmail
                }
            }
        }
      };

      var queryResults = await _dynamoDb.QueryAsync(query);
      foreach(var item in queryResults.Items)
      {
        allRequests.Add(_utils.ToObjectFromDynamoResult<LeaveRequest>(item));
      }
      return allRequests;
    }

    public async Task<LeaveRequest> AddUpdateLeaveRequest(LeaveRequest request)
    {
      PutItemRequest putRequest = new PutItemRequest()
      {
        TableName = _config.GetValue<string>("DynamoDbTables:LeaveRequestsTableName"),
        Item = _utils.ToDynamoAttributeValueDictionary<LeaveRequest>(request)
      };

      await _dynamoDb.PutItemAsync(putRequest);
      return request;
    }    

  }
}
