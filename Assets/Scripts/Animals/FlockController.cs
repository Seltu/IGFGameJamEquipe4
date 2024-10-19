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
    [SerializeField] private float _animalNeighborRadius = 3f;
    [SerializeField] private Transform _mouseTarget;
    private List<Transform> _selectedEnemies = new();
    private List<AnimalController> _animals = new();
    private float _assignTimer = 0.2f;
    private bool _selectingEnemies;
    private int _distributionIndex;
    private bool _freeAnimals;

    private void Start()
    {
        EventManager.onDeathEvent += CheckTargetDeath;
        for (int i = 0; i < _initialAmount; i++) {
            var animal = Instantiate(_animalPrefabs[Random.Range(0, _animalPrefabs.Count)], transform.position, Quaternion.identity);
            animal.SetTarget(transform);
            _animals.Add(animal);
        }
    }

    private void OnDestroy()
    {
        EventManager.onDeathEvent -= CheckTargetDeath;
    }

    private void CheckTargetDeath(GameObject dead)
    {
        foreach (var animal in _animals)
        {
            if(animal.GetTarget().Equals(dead.transform))
            {
                animal.SetTarget(transform);
                _selectedEnemies.Remove(dead.transform);
                if (_selectedEnemies.Count > 0)
                    _freeAnimals = true;
                else
                    _selectingEnemies = false;
            }
        }
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
        if (Input.GetMouseButtonUp(0))
        {
            foreach (var animal in _animals)
            {
                animal.SetTarget(transform);
                animal.SetSpeed(2f);
            }
            _selectingEnemies = false;
            _selectedEnemies.Clear();
        }
        if(Input.GetMouseButton(0))
        {
            if (_assignTimer <= 0)
            {
                if (_selectingEnemies)
                {
                    if (_distributionIndex < _animals.Count&&_selectedEnemies.Count>0)
                    {
                        var currentEnemy = _selectedEnemies[_distributionIndex % _selectedEnemies.Count];
                        if (_animals[_distributionIndex].GetTarget() != currentEnemy)
                        {
                            _animals[_distributionIndex].SetTarget(currentEnemy);
                            _animals[_distributionIndex].SetSpeed(3f);
                            _assignTimer = _assignTime;
                        }
                        _distributionIndex++;
                    }
                }
                else
                {
                    var freeAnimals = _animals.Where(obj => obj.GetTarget().Equals(transform));
                    if (freeAnimals.Count() > 0)
                    {
                        var animal = freeAnimals.OrderBy(obj => (obj.transform.position - _mouseTarget.position).sqrMagnitude).FirstOrDefault();
                        animal.SetTarget(_mouseTarget);
                        animal.SetSpeed(3f);
                        _assignTimer = _assignTime;
                    }
                }
             }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Ground"))
                    _mouseTarget.position = hit.point;
                var aroundHit = Physics.OverlapSphere(hit.point, 3f);
                foreach (var enemy in aroundHit)
                {
                    if (enemy.CompareTag("Enemy"))
                    {
                        if (!_selectedEnemies.Contains(enemy.transform)||_freeAnimals)
                        {
                            _selectedEnemies.Add(enemy.transform);
                            _selectingEnemies = true;
                            _distributionIndex = 0;
                            _assignTimer = 0;
                            _freeAnimals = false;
                        }
                    }
                }
            }
        }
    }
}
