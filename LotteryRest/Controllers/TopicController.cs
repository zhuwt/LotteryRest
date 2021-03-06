﻿using LotteryRest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LotteryRest.Controllers
{
    public class TopicController : ApiController
    {
        // GET: api/Topic
        public DTO.ReturnJasonConstruct<List<DTO.Topic>> Get()
        {
            try
            {
                string[] separators = { "@||@"};
                DTO.ReturnJasonConstruct<List<DTO.Topic>> dto = new DTO.ReturnJasonConstruct<List<DTO.Topic>>();
                dto.status = (int)DTO.executeStatus.success;
                dto.DTOObject = new List<DTO.Topic>();
                UserInformationEntities db = new UserInformationEntities();
                var objectList = db.TopicStack.ToList();
                foreach (var item in objectList)
                {
                    DTO.Topic obj = new DTO.Topic();
                    obj.topic = item.Topic;
                    obj.choices = new List<DTO.Choice>();
                    string[] choices = item.Choices.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    string[] goal = item.Goals.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    for (int n=0;n< choices.Length;n++)
                    {
                        obj.choices.Add(new DTO.Choice
                        {
                            choice = choices[n],
                            goal = int.Parse(goal[n])
                        });
                    }
                    dto.DTOObject.Add(obj);
                }
                return dto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: api/Topic/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Topic
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Topic/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Topic/5
        public void Delete(int id)
        {
        }
    }
}
