using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class FlockController : MonoBehaviour
{
    [SerializeField] private List<AnimalController> _animalPrefabs;
    [SerializeField] private int _initialAmount;
    [SerializeField] private float _assignTime;
    [SerializeField] private float _standByTime;
    [SerializeField] private float _animalNeighborRadius = 3f;
    [SerializeField] private Transform _mouseTarget;
    [SerializeField] private AudioSource enemySelect;
    [SerializeField] private AudioSource animalAttack;
    [SerializeField] private AudioSource HitSound;
    [SerializeField] private AudioSource CreateAnimalSound;
    [SerializeField] private Texture2D specialCursor;
    private List<Transform> _selectedEnemies = new();
    private List<AnimalController> _animals = new();
    private float _assignTimer = 0.2f;
    private float _standByTimer = 0;
    private bool _selectingEnemies;
    private int _distributionIndex;
    private int _totalAnimalsDiscarded;

    private void Start()
    {
        Cursor.SetCursor(specialCursor, Vector2.zero, CursorMode.Auto);
        EventManager.onDeathEvent += CheckTargetDeath;
        EventManager.onCreateNewAnimalEvent += CreateAnimal;
        EventManager.onPlayerGotHitEvent += OnPlayerHitDiscard;
        EventManager.onCombatStartEvent += Reunite;

        for (int i = 0; i < _initialAmount; i++)
        {
            CreateAnimal(this.transform);
        }
    }

    private void Reunite()
    {
        _standByTimer = _standByTime;
        foreach (var animal in _animals)
        {
            animal.SetTarget(transform);
            animal.SetCollider(false);
            animal.SetSpeed(4f);
        }
        _selectingEnemies = false;
        _selectedEnemies.Clear();
    }

    private void OnDestroy()
    {
        EventManager.onDeathEvent -= CheckTargetDeath;
        EventManager.onCreateNewAnimalEvent -= CreateAnimal;
        EventManager.onPlayerGotHitEvent -= OnPlayerHitDiscard;
        EventManager.onCombatStartEvent -= Reunite;
    }

    private void CreateAnimal(Transform pos)
    {
        var animal = Instantiate(_animalPrefabs[Random.Range(0, _animalPrefabs.Count)], pos.position, Quaternion.identity);
            animal.SetTarget(pos);
            animal.SetCollider(false);
            _animals.Add(animal);
            CreateAnimalSound.Play();

        EventManager.OnUpdateAnimalCountTrigger(_animals.Count);
    }

    private void CheckTargetDeath(GameObject dead)
    {
        foreach (var animal in _animals)
        {
            if(animal.GetTarget().Equals(dead.transform))
            {  
                animal.SetTarget(transform);
                animal.SetCollider(false);
                _selectedEnemies.Remove(dead.transform);
                if (_selectedEnemies.Count <= 0)
                    _selectingEnemies = false;
                _distributionIndex = 0;
            }
        }

        //if(dead.CompareTag("Enemy"))
            //CreateAnimal(dead.transform);
    }

    private void CheckAnimalNeighbors()
    {
        foreach (var animal in _animals)
        {
            List<AnimalController> neighbors = new List<AnimalController>();
            foreach (var animal2 in _animals)
            {
                if (Vector3.Distance(animal.transform.position, animal2.transform.position)<=_animalNeighborRadius&&animal2.gameObject != animal.gameObject&&animal.GetTarget().Equals(animal2.GetTarget()))
                {
                    neighbors.Add(animal2);
                }
            }
            animal.SetNeighbors(neighbors);
        }
    }

    private void Update()
    {
        if(_assignTimer>0)
            _assignTimer -= Time.deltaTime;
        CheckAnimalNeighbors();
        if(_standByTimer > 0) {
            _standByTimer -= Time.deltaTime;
            return;
        }
        if (Input.GetMouseButtonDown(0)||(Input.GetMouseButtonUp(0)&&_selectedEnemies.Count<=0))
        {
            foreach (var animal in _animals)
            {
                animal.SetTarget(transform);
                animal.SetCollider(false);
                animal.SetSpeed(2f);
            }
            _selectingEnemies = false;
            _selectedEnemies.Clear();
        }
        if (_assignTimer <= 0&&_selectingEnemies)
        {
            if (_distributionIndex < _animals.Count && _selectedEnemies.Count > 0)
            {
                var currentEnemy = _selectedEnemies[_distributionIndex % _selectedEnemies.Count];
                if (_animals[_distributionIndex].GetTarget() != currentEnemy)
                {
                    _animals[_distributionIndex].SetTarget(currentEnemy);
                    _animals[_distributionIndex].SetCollider(true);
                    _animals[_distributionIndex].SetSpeed(3f);
                    _assignTimer = _assignTime;
                }
                animalAttack.Play();
                _distributionIndex++;
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (_assignTimer <= 0&&!_selectingEnemies)
            {
                var freeAnimals = _animals.Where(obj => obj.GetTarget().Equals(transform));
                if (freeAnimals.Count() > 0)
                {
                    var animal = freeAnimals.OrderBy(obj => (obj.transform.position - _mouseTarget.position).sqrMagnitude).FirstOrDefault();
                    animal.SetTarget(_mouseTarget);
                    animal.SetCollider(true);
                    animal.SetSpeed(3f);
                    _assignTimer = _assignTime;
                }
            }

            if (Time.timeScale <= 0) return;
                
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Ground"))
                    _mouseTarget.position = hit.point;
                var aroundHit = Physics.OverlapSphere(hit.point, 1f);
                foreach (var enemy in aroundHit)
                {
                    if (enemy.CompareTag("Enemy") || enemy.CompareTag("Door"))
                    {
                        if (!_selectedEnemies.Contains(enemy.transform))
                        {
                            enemySelect.Play();
                            _selectedEnemies.Add(enemy.transform);
                            _selectingEnemies = true;
                            _distributionIndex = 0;
                            _assignTimer = 0;
                        }
                    }
                }
            }
        }
    }

    private void OnPlayerHitDiscard()
    {
        HitSound.Play();

        if(_animals.Count <= 6 && (_animals.Count - 3) > 0)
        {
            _totalAnimalsDiscarded = 3;
        }
        else if(_animals.Count <= 6 && (_animals.Count - 3) <= 0)
        {
            _totalAnimalsDiscarded = _animals.Count;
        }
        else
        {
            _totalAnimalsDiscarded = _animals.Count / 2;
        }

        for(int i = 0; i < _totalAnimalsDiscarded; i++)
        {
            StartCoroutine(DiscardAnimals());
        }
        
        EventManager.OnUpdateAnimalCountTrigger(_animals.Count);

        if(_animals.Count <= 0)
        {
            EventManager.OnGameOverTrigger();
        }
    }

    private IEnumerator DiscardAnimals()
    {
        AnimalController tempAnimal = _animals[0];

        tempAnimal.AnimalDeath();
        tempAnimal.GetThrowObject().InpulseThrow();
        _animals.RemoveAt(0);

        yield return new WaitForSeconds(2);
        Destroy(tempAnimal.gameObject);        
    }
}
