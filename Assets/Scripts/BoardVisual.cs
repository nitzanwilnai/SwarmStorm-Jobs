using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonTools;

namespace Swarm
{
    public class BoardVisual : MonoBehaviour
    {
        public SpriteRenderer SwarmPrefab;
        public SpriteRenderer Player;

        public GameObject MoveCircle;
        public Transform InsideCircle;

        Vec2 m_mouseDownPos;
        Vec2 m_prevPosition;
        Vector3 m_screenPosition;

        Camera m_mainCamera;
        public Transform SwarmParent;

        public SpriteRenderer[] Enemies;
        public Transform[] EnemyTransforms;
        public FrameAnimation[] FrameAnimations;

        public int FrameTime;
        float m_time;
        int m_counter;

        // Start is called before the first frame update
        public void Init(Camera mainCamera)
        {
            m_mainCamera = mainCamera;
        }

        public void Show(GameData gameData, Balance balance)
        {
            Enemies = new SpriteRenderer[gameData.EnemyCount];
            EnemyTransforms = new Transform[gameData.EnemyCount];
            FrameAnimations = new FrameAnimation[gameData.EnemyCount];
            for (int i = 0; i < gameData.EnemyCount; i++)
            {
                SpriteRenderer enemy = Instantiate(SwarmPrefab, SwarmParent).GetComponent<SpriteRenderer>();
                Enemies[i] = enemy;
                EnemyTransforms[i] = enemy.transform;
                FrameAnimations[i] = enemy.GetComponent<FrameAnimation>();
            }
        }

        public void Hide()
        {
            for (int i = 0; i < Enemies.Length; i++)
                GameObject.Destroy(Enemies[i].gameObject);
            Enemies = null;
        }

        public void Tick(GameData gameData, Balance balance, float dt)
        {
            handleInput(gameData);
            BoardLogic.Tick(gameData, balance, dt);
            syncVisuals(gameData);
        }

        void syncVisuals(GameData gameData)
        {
            Player.transform.localPosition = Vec2.ToVector3(gameData.PlayerPos);

            m_time += Time.deltaTime;
            if (m_time > FrameTime)
            {
                m_time -= FrameTime;
                for (int i = 0; i < gameData.EnemyCount; i++)
                    FrameAnimations[i].FrameChanged();
            }
        }

        void handleInput(GameData gameData)
        {
#if UNITY_EDITOR
            bool mouseDown = Input.GetMouseButtonDown(0);
            bool mouseMove = Input.GetMouseButton(0);
            bool mouseUp = Input.GetMouseButtonUp(0);
            Vector3 mousePosition = Input.mousePosition;
#else
            bool mouseDown = (Input.touchCount > 0) && Input.GetTouch(0).phase == TouchPhase.Began;
            bool mouseMove = (Input.touchCount > 0) && Input.GetTouch(0).phase == TouchPhase.Moved;
            bool mouseUp = (Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled);
            Vector3 mousePosition = Vector3.zero;
            if (Input.touchCount > 0)
                mousePosition = Input.GetTouch(0).position;
#endif
            Vector3 worldPosition = m_mainCamera.ScreenToWorldPoint(mousePosition);
            Vec2 localPos = new Vec2(SwarmParent.transform.InverseTransformPoint(worldPosition));

            if (mouseDown)
            {
                MoveCircle.SetActive(true);
                MoveCircle.transform.localPosition = Vec2.ToVector3(localPos);

                m_mouseDownPos = localPos;
                m_prevPosition = m_mouseDownPos;
            }
            if (mouseMove)
            {
                BoardLogic.SetPlayerMoveDirection(gameData, localPos - m_mouseDownPos);

                InsideCircle.localPosition = Vec2.ToVector3(gameData.PlayerDir * 0.25f);

                m_prevPosition = localPos;
            }
            if (mouseUp)
            {
                MoveCircle.SetActive(false);
                gameData.PlayerDir = Vec2.Zero();
            }
        }
    }
}