//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//namespace Game.CommandUI
//{
//    public abstract class Command
//    {
//        public Command nextCommand;
        
//        public void LinkTo(Command next)
//        {
//            Debug.Log("linking to " + next);
//            nextCommand = next;
//        }
//        public abstract void Execute();
//    }

//    public class StartCommand : Command
//    {
//        public override void Execute()
//        {
//            Debug.Log("Start command executed");
//            if (nextCommand != null)
//            {
//                Debug.Log("Next command executed");
//                nextCommand.Execute();
//            }
//        }
//    }
//    public class MoveCommand : Command
//    {
//        public string direction;
//        public MoveCommand(string direction)
//        {
//            this.direction = direction;
//        }
//        public override void Execute()
//        {
//            Debug.Log("Move " + direction);
//            if (nextCommand != null)
//            {
//                nextCommand.Execute();
//            }
//        }
//    }

//    public class IfCommand : Command
//    {
//        public string condition;
//        public Command ifTrue;
//        public Command ifFalse;
//        public IfCommand(string condition, Command ifTrue, Command ifFalse)
//        {
//            this.condition = condition;
//            this.ifTrue = ifTrue;
//            this.ifFalse = ifFalse;
//        }
//        public override void Execute()
//        {
//            Debug.Log("If " + condition);
//            if (condition == "true")
//            {
//                ifTrue.Execute();
//            }
//            else
//            {
//                ifFalse.Execute();
//            }
//        }
//    }

//    public class CommandSystem : MonoBehaviour
//    {
//        public Command root;
//        public static TMP_Text sequence;
//        public static string sequenceText;

//        // Start is called before the first frame update
//        void Start()
//        {

//        }

//        private void Awake()
//        {
//            root = new StartCommand();
//            Command A = new MoveCommand("Up");

//            //Command A = Instantiate(new MoveCommand("up"));
//            //Command B = Instantiate(new MoveCommand("left"));
//            //Command C = Instantiate(new MoveCommand("down"));

//            root.LinkTo(A);
//            Debug.Log(root.nextCommand);
//            //A.LinkTo(B);
//            //B.LinkTo(C);
//            root.Execute();

//        }
//        // Update is called once per frame
//        void Update()
//        {
            
//        }
        
//        public static void AddText(string Text)
//        {
//            sequenceText += Text;
//        }
//    }
//}

