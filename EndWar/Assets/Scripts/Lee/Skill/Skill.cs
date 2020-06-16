using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//스킬 타입
public enum SkillType {RADIAL, POINT, NONTARGET }

//스킬의 효과
public enum Skillability {NONE, STUN, DOT, SLOW, MEZ }

[RequireComponent(typeof(PhotonView))]
public class Skill : MonoBehaviourPun
{
    public Transform player;

    //공용
    public string skillName;    // 스킬 이름
    public float distance;      // 거리
    public int n_damageCount;   //대미지 배열 초기화
    public int[] damage;      //대미지
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
    public int speed;           //날아가는 스피드
    public float seconds;       //지속되는 시간
    public int nonTargetDamage; //논타겟팅 대미지
    public Quaternion rotation;  //네트워크 소환시 방향

    //MEZ 효과
    public bool electricShock;  //감전
    public bool freezing;       //빙결

    int n_count = 0;
    List<Transform> monsters; //자료형 몬스터 스크립트로 바꿀 것 (데미지를 줘야됨)  //나중에 바꿀것 몬스터로!!

    Transform thisTr;       //본인의 Transform 을 가질 변수

    void Start()
    {
        monsters = new List<Transform>();
        thisTr = GetComponent<Transform>();
        if(type == SkillType.NONTARGET)
            NonTargeting();
    }

    public void Use()
    {
        if (speed == null && type == SkillType.NONTARGET)
            return; 

        PhotonNetwork.Instantiate(gameObject.name, target, rotation);
    }

    //에디터용 필요없음
    public void ShowRange()
    {
        thisTr = GetComponent<Transform>();
        target = transform.position;
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
        }

        for (int i = 0; i < monsters.Count; i++)
        {
            Monster mon = monsters[i].GetComponent<Monster>();
            mon.GetDamage(damage[i]);
            mon.PlayerTarget(player);
            Ability(monsters[i],damage[i]);

        }
        n_count++;

        if (n_count == n_damageCount)
            n_count = 0;
    }

    void Ability(Transform monster, int damage)
    {
        Monster mon = monster.GetComponent<Monster>();
        switch (ability)
        {
            case Skillability.DOT:

                break;
            case Skillability.MEZ:
                if (electricShock)
                {

                }
                if (freezing)
                {

                }
                break;
            case Skillability.SLOW:
                mon.GetAbility((int)Skillability.SLOW, seconds, damage, slowing);
                break;
            case Skillability.STUN:
                mon.GetAbility((int)Skillability.STUN, seconds, damage);
                break;
        }
    }

    //애니메이션 이벤트용 함수
    private void DestroyObj()
    {
        Destroy(this.gameObject);
    }

    //논타겟형 함수들
    public void NonTargeting()
    {
        ProjectileMoveScript trigger = GetComponent<ProjectileMoveScript>();
        trigger.speed = speed;
        trigger.damage = nonTargetDamage;
        trigger.seconds = seconds;
    }


    //범위형 함수들
    private void AreaOfEffect()
    {
        Collider[] coll = Physics.OverlapSphere(target, range);
        if (!isCollision)
        {
            for (int i = 0; i < coll.Length; i++)
            {
                if (coll[i].gameObject.layer == LayerMask.NameToLayer("Monster")) //레이어로 바꿀 것
                {
                    monsters.Add(coll[i].transform);  //몬스터로 바꿀것
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
        _angle += thisTr.eulerAngles.y;

        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    private void View()
    {
        Vector3 playerPosition = target;

        Collider[] _target = Physics.OverlapSphere(playerPosition, distance, layerMask); //레이어를 몬스터로 설정

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if(_targetTf.gameObject.layer == LayerMask.NameToLayer("Monster")) //레이어
            {
                Vector3 _direction = (_targetTf.position - playerPosition).normalized;
                float _angle = Vector3.Angle(_direction, thisTr.forward);

                if (_angle < viewAngle * 0.5)
                {
                    RaycastHit _hit;
                    if (Physics.Raycast(playerPosition, _direction, out _hit, distance))
                    {
                        if (_hit.transform.gameObject.layer == LayerMask.NameToLayer("Monster")) // 레이어로 바꿀 것
                        {
                            monsters.Add(_hit.transform);  //몬스터로 바꿀것
                        }
                    }
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        Vector3 leftBoundary = BoundaryAngle(-viewAngle * .5f);
        Vector3 rightBounday = BoundaryAngle(viewAngle * .5f);
        Vector3 _distance = thisTr.forward * distance;

        switch (type)
        {
            case SkillType.RADIAL : 
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(thisTr.position, distance);
                Gizmos.color = Color.red;
                Gizmos.DrawRay(thisTr.position, leftBoundary * distance);
                Gizmos.DrawRay(thisTr.position, rightBounday * distance);
                break;
            case SkillType.POINT :
                Gizmos.color = color;
                Gizmos.DrawWireSphere(target, range);
                Gizmos.color = Color.red;
                Gizmos.DrawRay(thisTr.position, _distance);
                break;
            case SkillType.NONTARGET :
                Gizmos.color = Color.red;
                Gizmos.DrawRay(thisTr.position, _distance);
                break;
        }
    }
}
