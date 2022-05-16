using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Map
{
    public class MapPlayerTracker : MonoBehaviour
    {
        public bool lockAfterSelecting = false;
        public float enterNodeDelay = 1f;
        public MapManager mapManager;
        public MapView view;
        private UiActive ui;
        private GameplayManager gameplay;

        public static MapPlayerTracker Instance;

        public bool Locked { get; set; }

        private void Awake()
        {
            Instance = this;
            ui = GameObject.Find("UIManager").GetComponent<UiActive>();
            gameplay = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        }

        public void SelectNode(MapNode mapNode)
        {
            if (Locked) return;

            // Debug.Log("Selected node: " + mapNode.Node.point);

            if (mapManager.CurrentMap.path.Count == 0)
            {
                // player has not selected the node yet, he can select any of the nodes with y = 0
                if (mapNode.Node.point.y == 0)
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
            else
            {
                var currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
                var currentNode = mapManager.CurrentMap.GetNode(currentPoint);

                if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.Node.point)))
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
        }

        private void SendPlayerToNode(MapNode mapNode)
        {
            Locked = lockAfterSelecting;
            mapManager.CurrentMap.path.Add(mapNode.Node.point);
            mapManager.SaveMap();
            view.SetAttainableNodes();
            view.SetLineColors();
            mapNode.ShowSwirlAnimation();

            DOTween.Sequence().AppendInterval(enterNodeDelay).OnComplete(() => EnterNode(mapNode));
        }

        private void EnterNode(MapNode mapNode)
        {
            // we have access to blueprint name here as well
            Debug.Log("Entering node: " + mapNode.Node.blueprintName + " of type: " + mapNode.Node.nodeType);
            // load appropriate scene with context based on nodeType:
            // or show appropriate GUI over the map: 
            // if you choose to show GUI in some of these cases, do not forget to set "Locked" in MapPlayerTracker back to false
            switch (mapNode.Node.nodeType)
            {
                case NodeType.MinorEnemy:
                    ui.OnMapClick();
                    StartCoroutine(gameplay.SetupBattle());
                    Locked = true;
                    break;
                case NodeType.EliteEnemy:
                    ui.OnMapClick();
                    StartCoroutine(gameplay.SetupEliteBattle());
                    Locked = true;
                    break;
                case NodeType.RestSite:
                    ui.OnMapClick();
                    StartCoroutine(gameplay.SetupRestSite());
                    Locked = true;
                    break;
                case NodeType.Treasure:
                    ui.OnMapClick();
                    StartCoroutine(gameplay.SetupTreasure());
                    Locked = true;
                    break;
                case NodeType.Store:
                    ui.OnMapClick();
                    StartCoroutine(gameplay.SetupStore());
                    Locked = true;
                    break;
                case NodeType.Boss:
                    ui.OnMapClick();
                    StartCoroutine(gameplay.SetupBoss());
                    Locked = true;
                    break;
                case NodeType.Mystery:
                    ui.OnMapClick();
                    StartCoroutine(gameplay.SetupMistery());
                    Locked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PlayWarningThatNodeCannotBeAccessed()
        {
            Debug.Log("Selected node cannot be accessed");
        }
    }
}