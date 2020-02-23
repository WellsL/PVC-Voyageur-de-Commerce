﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using Microsoft.AspNetCore.Mvc;
using PTS.API;
using PTS.API.Result.Setup;
using PTS.App.Managers;
using PTS.App.Objects;
using PTS.App.SelectionMetodes;

namespace PTS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SetupController : ControllerBase
    {
        private CityManager cityManager = new CityManager();

        [HttpGet]
        public string GetToken()
        {
            //Create new session
            int token = SessionManager.NewSession();

            //Get cityNames
            List<Tuple<string, string>> cityList = cityManager.GetAllCitiesName();

            //Get list of the selection methodes available and their default mutate factor
            Type methodeType;
            SelectionMethode selectionMethode;
            List<Tuple<string, double>> selectionMethodes = new List<Tuple<string, double>>();

            foreach (ESelectionMethodes eMethode in Enum.GetValues(typeof(ESelectionMethodes)))
            {
                Console.WriteLine(eMethode.ToString());

                //Get the type of the selection methodes
                methodeType = Type.GetType("PTS.App.SelectionMetodes." + eMethode.ToString());

                //Instanciate the selectionMethode
                selectionMethode = (SelectionMethode)Activator.CreateInstance(methodeType);
                selectionMethodes.Add(new Tuple<string, double>(eMethode.ToString(), selectionMethode.mutateFactor));

            }

            RGetToken result = new RGetToken(token, selectionMethodes, cityList);
            //Serialization to JSON


            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(RGetToken));
            MemoryStream msObj = new MemoryStream();
            js.WriteObject(msObj, result);
            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj);

            string json = sr.ReadToEnd();

            sr.Close();
            msObj.Close();

            return json;
        }
    }
}
