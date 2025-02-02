using System;
using System.Collections.Generic;
using System.Linq;
using GameDevTV.Utils;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] string playerName = "";
        Dialogue currentDialogue = null;
        DialogueNode currentNode = null;
        AIConversant currentConversant = null;
        bool isChoosing = false;

        public event Action onConversationUpdated;

        public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
        {
            currentConversant = newConversant;
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
            onConversationUpdated?.Invoke();
        }

        public void Quit()
        {
            currentDialogue = null;
            TriggerExitAction();
            currentNode = null;
            isChoosing = false;
            currentConversant = null;
            onConversationUpdated?.Invoke();
        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public string GetText()
        {
            if(currentNode == null) return "";

            return currentNode.GetText();
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            TriggerEnterAction();
            isChoosing = false;
            Next();
        }

        public void Next()
        {
            if(!HasNext())
            {
                Quit();
                return;
            }

            int numPlayerResponses = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();

            if(numPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitAction();
                onConversationUpdated?.Invoke();
                return;
            }

            DialogueNode[] children = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
            
            TriggerExitAction();

            if(children.Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, children.Count());
                currentNode = children[randomIndex];
            }

            TriggerEnterAction();

            onConversationUpdated?.Invoke();
        }

        public bool HasNext()
        {
            return FilterOnCondition(currentDialogue.GetAllChildren(currentNode)).Count() > 0;
        }

        public string GetCurrentConversantName()
        {
            if(isChoosing)
            {
                return playerName;
            }
            else
            {
                return currentConversant.GetName();
            }
        }

        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
        {
            foreach(var node in inputNode)
            {
                if(node.CheckCondition(GetEvaluators()))
                {
                    yield return node;
                }
            }
        }

        private IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
        }

        private void TriggerEnterAction()
        {
            if(currentNode != null)
            {
                foreach(string action in currentNode.GetOnEnterActions())
                {
                    TriggerAction(action);
                }
            }
        }

        private void TriggerExitAction()
        {
            if(currentNode != null)
            {
                foreach(string action in currentNode.GetOnExitActions())
                {
                    TriggerAction(action);
                }
            }
        }

        private void TriggerAction(string action)
        {
            if(action == "") return;

            foreach(DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(currentConversant.GetDialogueName(), action);
            }
        }
    }
}