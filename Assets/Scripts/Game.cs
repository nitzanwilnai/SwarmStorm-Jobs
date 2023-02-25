using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonTools;
namespace Swarm
{
    public class Game : Singleton<Game>
    {
        public enum MENU_STATE { LOADING, MAIN_MENU, IN_GAME };
        public MENU_STATE MenuState = MENU_STATE.LOADING;

        public Camera MainCamera;

        public BoardVisual BoardVisual;

        GameData m_gameData = new GameData();
        Balance m_balance = new Balance();

        public EnemyMoveSystem EnemyMoveSystem;

        public uint Seed;

        protected override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;
        }

        // Start is called before the first frame update
        void Start()
        {
            m_balance.LoadBalance();

            BoardVisual.Init(MainCamera);
            MenuState = MENU_STATE.IN_GAME;
            BoardLogic.Allocate(m_gameData, m_balance);
            BoardLogic.StartGame(m_gameData, m_balance, ref Seed);
            BoardVisual.Show(m_gameData, m_balance);
            EnemyMoveSystem.InitTransforms(BoardVisual.EnemyTransforms);
        }

        // Update is called once per frame
        void Update()
        {
            if(MenuState == MENU_STATE.IN_GAME)
            {
                BoardVisual.Tick(m_gameData, m_balance, Time.deltaTime);
                EnemyMoveSystem.Tick(m_gameData);
            }
        }
    }
}