using System;
using System.Collections.Generic;
using UnityEngine;

namespace nseh.GameManager
{
    public class Conflict
    {
        private GameObject sender;
        private GameObject enemy;

        #region Public Properties

        public GameObject Sender
        {
            get
            {
                return this.sender;
            }
        }

        public GameObject Enemy
        {
            get
            {
                return this.enemy;
            }
        }

        #endregion

        public Conflict(GameObject sender, GameObject enemy)
        {
            this.sender = sender;
            this.enemy = enemy;
        }
        
        public bool ResolveConflict()
        {
            bool isResolved = false;



            return isResolved;
        }
    }

    public class CombatManager : Service
    {
        private List<Conflict> conflicts;

        public override void Activate()
        {
            this.conflicts = new List<Conflict>();

            this.IsActivated = true;
        }

        public override void Release()
        {
            throw new NotImplementedException();
        }

        public override void Tick()
        {
            throw new NotImplementedException();
        }

        public void AddConflict(Conflict newConflict)
        {
            foreach(Conflict conflict in this.conflicts)
            {
                if (!EqualConflicts(conflict, newConflict))
                {
                    this.conflicts.Add(newConflict);
                }
                else
                {
                    Debug.Log("The conflict already exists!");
                }
            }
        }

        public void ClearConflicts()
        {
            this.conflicts.Clear();
        }

        public bool EqualConflicts(Conflict conflict1, Conflict conflict2)
        {
            return conflict1.Sender == conflict2.Enemy && conflict1.Enemy == conflict2.Sender;
        }
    }
}
