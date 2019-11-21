﻿using System;
using System.Collections.Generic;
using PTS.App.Objects;

namespace PTS.App.Utils
{
    public static class SelectionMethodes
    {
        private static Random random = new Random();

        public static Journey Tournament(List<Journey> journeys, int size)
        {
            List<int> Participant = new List<int>();

            int index = -1;
            int rIndex;

            for ( int i = 0; i < size; i++)
            {
                do
                {
                    rIndex = random.Next(0, journeys.Count);
                } while (Participant.Contains(rIndex));

                Participant.Add(rIndex);

                if ( index != -1 && journeys[index].GetFitness() > journeys[rIndex].GetFitness())
                {
                    index = rIndex;
                }
            }

            return journeys[index];
        }
    }
}
