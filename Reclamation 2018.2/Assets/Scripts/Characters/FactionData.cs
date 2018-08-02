using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Characters
{
    public enum ReputationLevel
    {
        Hated, Neutral, Friendly,
        Number, None
    }

    public class FactionData
    {
        public new string name;
        public string key;

        public List<Reputation> reputations;

        public FactionData()
        {
            name = "";
            reputations = new List<Reputation>();
        }

        public FactionData(string name, string key)
        {
            reputations = new List<Reputation>();

            this.name = name;
            this.key = key;
        }
    }

    public class Reputation
    {
        public ReputationLevel level;

        public Reputation()
        {
            level = ReputationLevel.None;
        }

        public Reputation(ReputationLevel level)
        {
            this.level = level;
        }
    }
}