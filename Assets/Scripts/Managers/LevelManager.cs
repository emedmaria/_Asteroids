using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace AsteroidsClone
{
	public class LevelManager : Singleton<LevelManager>
	{
		private const int FX_POOL_SIZE = 10; 
		private const int UNITS_POOL_SIZE = 7;

		[SerializeField]
		[Header("Player Ship Prefab")]
		private GameObject m_playerShipPrefab; //Reference to the Ship that the player will control
		[SerializeField]
		[Header("Small Asteroid Prefab")]
		private GameObject m_asteroidSmallPrefab; //Reference to the small Asteroid prefab
		[SerializeField]
		[Header("Big Asteroid Prefab")]
		private GameObject m_asteroidBigPrefab;

		[SerializeField]
		[Header("Enemy Types Prefabs")]
		private GameObject[] enemiesPrefab;

		private Detonator detonator; //	Explosions/Particle Sys

		//	Single Player
		private Player player;
		//private PlayerCollisionDetection m_playerCollisionDetection;
		private BasicCollisionDectection m_playerCollisionDetection;

		//	Pooled Objects
		private ObjectPool<GameObject> m_asteroidBigPool; 
		private ObjectPool<GameObject> m_asteroidSmallPool;

        //  Spawners
        private BaseSpawner m_asteroidsSmallSpawner;
        private BaseSpawner m_asteroidsBigSpawner;

		private EnemySpawner m_enemySpawner;

		//private int m_level;

		#region - Events to suscribe with
		public event EventHandler<CollisionDetectedEventArgs> Scored;
		protected void OnScored(object sender, CollisionDetectedEventArgs e)
		{
			if (Scored != null)
				Scored(sender, e);
		}

		public event EventHandler Died;
		protected void OnDied(object sender, EventArgs e)
		{
			if (Died != null)
				Died(this, EventArgs.Empty);
		}
		#endregion

		#region - Monobehaviour methods
		public override void Awake()
		{
			//	Ensure prefabs are set 
			Assert.IsNotNull(m_playerShipPrefab, "[LevelManager] PlayerPrefab must be referenced in the inspector!");
			Assert.IsNotNull(m_asteroidSmallPrefab, "[LevelManager] asteroidPrefab must be referenced in the inspector!");
			Assert.IsNotNull(m_asteroidBigPrefab, "[LevelManager] asteroidPrefab must be referenced in the inspector!");

			//	References
			detonator = Detonator.Instance;

            //DontDestroyOnLoad(this.gameObject);
            base.Awake(); 
		}
        #endregion

        public void Initialize()
		{
			//	All dynamic objects nested in the Dynamic GO
			Transform parent = GameObject.FindGameObjectWithTag(Tags.Dynamic).transform;

			//	Player
			player = Player.Spawn(m_playerShipPrefab, parent);
			m_playerCollisionDetection = player.GetComponentInChildren<BasicCollisionDectection>();
			//m_playerCollisionDetection = player.GetComponentInChildren<PlayerCollisionDetection>();

			AddListeners(); 

			player.Destruction();

			//	Pool of asteroids
			PoolManager.BuildPool(m_asteroidBigPrefab, UNITS_POOL_SIZE);
			m_asteroidBigPool = PoolManager.GetPool(m_asteroidBigPrefab); 

			PoolManager.BuildPool(m_asteroidSmallPrefab, UNITS_POOL_SIZE);
			m_asteroidSmallPool = PoolManager.GetPool(m_asteroidSmallPrefab);

			//m_asteroidBigPool = ObjectPool.Build(m_asteroidBigPrefab, 25);
			//m_asteroidSmallPool = ObjectPool.Build(m_asteroidSmallPrefab, 25);

			// Pool of explosions
			detonator.BuildPools(FX_POOL_SIZE); 
		}

		private void AddListeners()
		{
			//	Listeners of events triggered by the player
			m_playerCollisionDetection.Collided += OnEntityCollided;
			m_playerCollisionDetection.UnitShot += OnUnitShot;

			//m_playerCollisionDetection.PlayerGetsShot += OnPlayerGetsShot;

			player.LivedDecreased += OnDied;
		}

		private void RemoveListeners()
		{
			m_playerCollisionDetection.Collided -= OnEntityCollided;
			m_playerCollisionDetection.UnitShot -= OnUnitShot;
			//m_playerCollisionDetection.PlayerGetsShot -= OnPlayerGetsShot;

			//asteroidsBigSpawner.Shot -= OnUnitShot;
			//asteroidsSmallSpawner.Shot -= OnUnitShot;
			player.LivedDecreased -= OnDied;
		}

		private bool check = false; 
		public void StartLevel(int level, int nAsteroidsByLevel)
		{
			//	Spawn Asteroids - Big
			//nAsteroidsByLevel = 1; //Test
			m_asteroidsBigSpawner = new BaseSpawner(nAsteroidsByLevel,m_asteroidBigPrefab, m_asteroidSmallPrefab);
			m_asteroidsBigSpawner.SpawnSet();

			//	Enemies Spawner
			/*m_enemySpawner = new EnemySpawner(enemiesPrefab);

			m_enemySpawner.StopSpawn = false;
			StartCoroutine(m_enemySpawner.StartSpawner(player.transform));

			//	Suscription to events related to Enemy Collisions
			m_enemySpawner.Shot += OnUnitShot;
			*/

			//	Suscription to events related to Unit Collisions
			m_asteroidsBigSpawner.Shot += OnUnitShot;
		}

		public  void RecoverPlayer()
		{
			player.Recover();
			player.EnableControls();
		}

		public void ResetPlayer() { player.Destruction(); }
		public void ResetEnemies() { m_enemySpawner.Reset(); }

		/// <summary>
		/// An Active/Passive unit has been collided (by the player)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnEntityCollided(object sender, CollisionDetectedEventArgs e)
		{
			//	VFX of PassiveEnemy (Asteroid) / ActiveEnemy (UFO)
			EnemyUnit unit = e.EnemyUnit;
			if (unit == null) return;  //	TODO: Recheck, this should not happen...
			unit.Collide();

			//	Player Collision
			e.SourceEntity.Collide(unit.Damage); 
		}

		/// <summary>
		/// A Unit has been shot by the player
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnUnitShot(object sender, CollisionDetectedEventArgs e)
		{
			//	VFX of PassiveEnemy (Asteroid) / ActiveEnemy (Saucer)
			EnemyUnit unit = e.EnemyUnit;
			unit.Collide();

			//	Player scored
			OnScored(sender, e);

			//	TODO Add nice animation with the Points Scored
		}

		private void OnPlayerGetsShot(object sender, CollisionDetectedEventArgs e)
		{
			EnemyUnit activeEnemy = e.EnemyUnit;

			//	Player Collision
			if (player == null) return; 
			player.Collide(activeEnemy.Damage);

			//	TODO Recicly Unit
			//m_enemySpawner.Reset();
		}

		public bool AsteroidsAlive { get { return m_asteroidsBigSpawner.EntityRemaining > 0; } }
		public int AsteroidsCount { get { return m_asteroidsBigSpawner.EntityRemaining; } }

		public void ResetSpawners()
		{
			if (m_asteroidsSmallSpawner != null)
			{
				m_asteroidsSmallSpawner.Shot -= OnUnitShot;
				m_asteroidsSmallSpawner.Reset();
			}

			if (m_asteroidsBigSpawner != null)
			{
				m_asteroidsBigSpawner.Shot -= OnUnitShot;
				m_asteroidsBigSpawner.Reset();
			}

			if (m_enemySpawner != null) {
				m_enemySpawner.Shot -= OnUnitShot;
				m_enemySpawner.Reset();
			}
		}
	}
}

