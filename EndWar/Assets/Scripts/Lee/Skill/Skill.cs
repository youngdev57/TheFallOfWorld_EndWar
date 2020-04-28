using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//스킬 타입
public enum SkillType {RADIAL, POINT, TARGET }

//스킬의 효과
public enum Skillability {NONE, AIRBORNE, STUN, DOT, SLOW, MEZ }

public class Skill : MonoBehaviour
{
    //공용
    public string skillName;    // 스킬 이름
    public Transform prefab;    //이펙트 오브젝트
    public float distance;      // 거리
    public int n_damageCount;   //대미지 배열 초기화
    public float[] damage;      //대미지
    public SkillType type;      // 스킬 타입
    public Skillability ability;// 상태효과

    //상태효과 변수
    public float abilityTime;   //지속시간
    public float slowing;       //둔화율

    //원뿔형
    public float viewAngle;     //시야각 (120도)

    //범위형
    public bool isCollision;    // 범위 안 물체 확인
    public float range;         //범위
    public LayerMask layerMask; //타켓 마스크(적)
    public Vector3 target;             //타켓의 위치
    Color color = Color.blue;   //Gizmo용 컬러

    //타겟형
    public Transform targeting; //타켓이 누가 잡혔는지에 대한 확인용

    //MEZ 효과
    public bool electricShock;  //감전
    public bool freezing;       //빙결

    private int n_count = 0;
    private List<Monster> monsters; //자료형 몬스터 스크립트로 바꿀 것 (데미지를 줘야됨)

    void Start()
    {
        monsters = new List<Monster>();
    }

    //차징용 함수
    public void ShowRange()
    {
        switch (type)
        {
            case SkillType.RADIAL:
                Vector3 leftBoundary = BoundaryAngle(-viewAngle * .5f);
                Vector3 rightBounday = BoundaryAngle(viewAngle * .5f);

                Debug.DrawRay(transform.position, leftBoundary * distance, Color.red);
                Debug.DrawRay(transform.position, rightBounday * distance, Color.red);
                break;
            case SkillType.POINT:
                Vector3 _distance = transform.forward * distance;
                Debug.DrawRay(transform.position, _distance, Color.red);
                break;
            case SkillType.TARGET:
                Debug.DrawRay(transform.position, transform.forward * distance, Color.red);
                break;
        }
    }

    //애니메이션 이벤트용 데미지 함수
    public void DoDamage()
    {
        monsters.Clear();
        switch (type)
        {
            case SkillType.RADIAL:
                View();
                break;
            case SkillType.POINT:
                AreaOfEffect();
                break;
            case SkillType.TARGET:
                Targeting();
                //targeting
                break;
        }

        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].GetDamage();
        }

        if (n_count == n_damageCount)
            n_count = 0;
    }

    //애니메이션 이벤트용 함수
    public void DestroyObj()
    {
        Destroy(this.gameObject);
    }

    //타겟형 함수들

    public void Targeting()
    {
        
    }


    //범위형 함수들

    public void AreaOfEffect()
    {
        Collider[] coll = Physics.OverlapSphere(target, range);
        if (!isCollision)
        {
            for (int i = 0; i < coll.Length; i++)
            {
                if (coll[i].gameObject.layer == LayerMask.NameToLayer("Water")) //레이어로 바꿀 것
                {
                    monsters.Add(coll[i].GetComponent<Monster>());
                }
            }
        }
        else
        {
            CollisionEnter();
        }
    }

    //범위 안 충돌체크 함수
    public bool CollisionEnter() // 공격용이 아님
    {
        Collider[] isColl = Physics.OverlapSphere(target, range, ~layerMask);
        if (isColl.Length > 0)
        {
            color = Color.red;
            return true;
        }
        else
        {
            color = Color.blue;
            return false;
        }
    }


    //원뿔형 함수들

    private Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    private void View()
    {
        Vector3 playerPosition = target;

        Collider[] _target = Physics.OverlapSphere(playerPosition, distance, layerMask); //레이어를 몬스터로 설정

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if(_targetTf.gameObject.layer == LayerMask.NameToLayer("Water")) //레이어
            {
                Vector3 _direction = (_targetTf.position - playerPosition).normalized;
                float _angle = Vector3.Angle(_direction, transform.forward);

                if (_angle < viewAngle * 0.5)
                {
                    RaycastHit _hit;
                    if (Physics.Raycast(playerPosition, _direction, out _hit, distance))
                    {
                        if (_hit.transform.gameObject.layer == LayerMask.NameToLayer("Water")) // 레이어로 바꿀 것
                        {
                            monsters.Add(_hit.transform.GetComponent<Monster>());
                        }
                    }
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        switch (type)
        {
            case SkillType.RADIAL : 
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, distance);
                break;
            case SkillType.POINT :
                Gizmos.color = color;
                Gizmos.DrawWireSphere(target,range);
                break;
            case SkillType.TARGET :

                break;
        }
    }
}
