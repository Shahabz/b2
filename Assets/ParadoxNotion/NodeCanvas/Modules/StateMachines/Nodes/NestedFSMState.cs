﻿using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.StateMachines{

	[Name("FSM")]
	[Category("Nested")]
	[Description("Execute a nested FSM OnEnter and Stop that FSM OnExit. This state is Finished when the nested FSM is finished as well")]
	public class NestedFSMState : FSMState, IGraphAssignable{

		[SerializeField]
		protected BBParameter<FSM> _nestedFSM = null; //protected so that derived user types can be reflected correctly
		private Dictionary<FSM, FSM> instances = new Dictionary<FSM, FSM>();

		public FSM nestedFSM{
			get {return _nestedFSM.value;}
			set {_nestedFSM.value = value;}
		}

		Graph IGraphAssignable.nestedGraph{
			get {return nestedFSM;}
			set {nestedFSM = (FSM)value;}
		}

		Graph[] IGraphAssignable.GetInstances(){ return instances.Values.ToArray(); }

		////

		protected override void OnEnter(){
			if (nestedFSM == null){
				Finish(false);
				return;
			}

			CheckInstance();
			nestedFSM.StartGraph(graphAgent, graphBlackboard, false, Finish);
		}

		protected override void OnUpdate(){
			nestedFSM.UpdateGraph();
		}

		protected override void OnExit(){
			if (IsInstance(nestedFSM) && (nestedFSM.isRunning || nestedFSM.isPaused) ){
				nestedFSM.Stop();
			}
		}

		protected override void OnPause(){
			if (IsInstance(nestedFSM)){
				nestedFSM.Pause();
			}
		}

		bool IsInstance(FSM fsm){
			return instances.Values.Contains(fsm);
		}

		void CheckInstance(){

			if (IsInstance(nestedFSM)){
				return;
			}

			FSM instance = null;
			if (!instances.TryGetValue(nestedFSM, out instance)){
				instance = Graph.Clone<FSM>(nestedFSM);
				instances[nestedFSM] = instance;
			}

            instance.agent = graphAgent;
		    instance.blackboard = graphBlackboard;
			nestedFSM = instance;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){
			GUILayout.Label(string.Format("Sub FSM\n{0}", _nestedFSM) );
			if (nestedFSM == null){
				if (!Application.isPlaying && GUILayout.Button("CREATE NEW")){
					Node.CreateNested<FSM>(this);
				}
			}
		}

		protected override void OnNodeInspectorGUI(){

			ShowBaseFSMInspectorGUI();
			EditorUtils.BBParameterField("FSM", _nestedFSM);
			
			if (nestedFSM == this.FSM){
				Debug.LogWarning("Nested FSM can't be itself!");
				nestedFSM = null;
			}

			if (nestedFSM == null){
				return;
			}

	    	var defParams = nestedFSM.GetDefinedParameters();
	    	if (defParams.Length != 0){

		    	EditorUtils.TitledSeparator("Defined Nested BT Parameters");
		    	GUI.color = Color.yellow;
		    	UnityEditor.EditorGUILayout.LabelField("Name", "Type");
				GUI.color = Color.white;
		    	var added = new List<string>();
		    	foreach(var bbVar in defParams){
		    		if (!added.Contains(bbVar.name)){
			    		UnityEditor.EditorGUILayout.LabelField(bbVar.name, bbVar.varType.FriendlyName());
			    		added.Add(bbVar.name);
			    	}
		    	}
		    	if (GUILayout.Button("Check/Create Blackboard Variables")){
		    		nestedFSM.CreateDefinedParameterVariables(graphBlackboard);
		    	}
		    }
		}
		
		#endif
	}
}