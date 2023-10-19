using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core {
    public class ActionScheduler : MonoBehaviour {

        IAction currentAction;
        public void StartAction(IAction action) {
            // cancel mover when combat starts
            // cancel combat when movement starts
            if (currentAction == action) return;
            if (currentAction != null) {
                print("Cancelling" + currentAction);
                // action.Cancel();
                currentAction.Cancel();
            }
            
            currentAction = action;
        }
    }
}