// using FridgeLogic.Patterns.FSM;
// using UnityEngine;

// namespace FridgeLogic.Control
// {
//     public class PlatformerCharacterController
//     {
//         private readonly IPlatformerInput _input;
//         private readonly IPlatformerCharacter _character;
//         private readonly StateMachine _stateMachine;

//         public PlatformerCharacterController(IPlatformerInput input, IPlatformerCharacter character)
//         {
//             _input = input;
//             _character = character;
//             _stateMachine = new StateMachine();

//             InitializeStateMachine();
//         }

//         private void InitializeStateMachine()
//         {
//             var idle = new PlatformerCharacterIdle();
//             var walk = new PlatformerCharacterWalk();
//             var run = new PlatformerCharacterRun();
//             var jump = new PlatformerCharacterJump();
//             var fall = new PlatformerCharacterFall();
//             var dead = new PlatformerCharacterDead();

//             // To Idle
//             _stateMachine.AddTransition(
//                 from: walk,
//                 to: idle,
//                 when: IsStill
//             );

//             // To Walk
//             _stateMachine.AddTransition(
//                 from: idle,
//                 to: walk,
//                 when: IsWalking
//             );

//             _stateMachine.AddTransition(
//                 from: run,
//                 to: walk,
//                 when: IsNotRunning
//             );

//             _stateMachine.AddTransition(
//                 from: fall,
//                 to: walk,
//                 when: IsNotFalling
//             );

//             // To Run
//             _stateMachine.AddTransition(
//                 from: walk,
//                 to: run,
//                 when: IsRunning
//             );

//             // To Jump
//             _stateMachine.AddTransition(
//                 from: walk,
//                 to: jump,
//                 when: IsJumping
//             );

//             _stateMachine.AddTransition(
//                 from: run,
//                 to: jump,
//                 when: IsJumping
//             );

//             // To Fall
//             _stateMachine.AddTransition(
//                 from: idle,
//                 to: fall,
//                 when: IsFalling
//             );

//             _stateMachine.AddTransition(
//                 from: walk,
//                 to: fall,
//                 when: IsFalling
//             );

//             _stateMachine.AddTransition(
//                 from: run,
//                 to: fall,
//                 when: IsFalling
//             );

//             // To Dead
//             _stateMachine.AddTransition(
//                 to: dead,
//                 when: IsDead
//             )
//         }

//         private bool IsStill() => _character.IsGrounded && _character.Velocity.magnitude == 0f;
//         private bool IsWalking() => _character.IsGrounded && Mathf.Abs(_character.Velocity.x) > 0f;
//         private bool IsRunning() => _character.IsGrounded && Mathf.Abs(_character.Velocity.x) >= _character.RunThreshold;
//         private bool IsNotRunning() => _character.IsGrounded && Mathf.Abs(_character.Velocity.x) < _character.RunThreshold;
//         private bool IsJumping() => !_character.IsGrounded && _character.Velocity.y > 0f;
//         private bool IsFalling() => !_character.IsGrounded && _character.Velocity.y <= 0f;
//         private bool IsNotFalling() => _character.IsGrounded;
//         private bool IsDead() => _character.IsDead;
//     }

//     public interface IPlatformerInput
//     {
//         Vector2 Movement { get; }
//         bool Jump { get; }
//         bool Run { get; }
//     }

//     public interface IPlatformerCharacter
//     {
//         Vector2 Velocity { get; }
//         float RunThreshold { get; }
//         bool IsGrounded { get; }
//         bool IsDead { get; }
//     }

//     public class PlatformerCharacterIdle : IState
//     {
//         public void OnEnter()
//         {
//             throw new System.NotImplementedException();
//         }

//         public void OnExit()
//         {
//             throw new System.NotImplementedException();
//         }

//         public void OnUpdate(float dt)
//         {
//             throw new System.NotImplementedException();
//         }
//     }

//     public class PlatformerCharacterWalk : IState
//     {
//         public void OnEnter()
//         {
//             throw new System.NotImplementedException();
//         }

//         public void OnExit()
//         {
//             throw new System.NotImplementedException();
//         }

//         public void OnUpdate(float dt)
//         {
//             throw new System.NotImplementedException();
//         }
//     }

//     public class PlatformerCharacterRun : IState
//     {
//         public void OnEnter()
//         {
//             throw new System.NotImplementedException();
//         }

//         public void OnExit()
//         {
//             throw new System.NotImplementedException();
//         }

//         public void OnUpdate(float dt)
//         {
//             throw new System.NotImplementedException();
//         }
//     }

//     public class PlatformerCharacterJump : IState
//     {
//         public void OnEnter()
//         {
//             throw new System.NotImplementedException();
//         }

//         public void OnExit()
//         {
//             throw new System.NotImplementedException();
//         }

//         public void OnUpdate(float dt)
//         {
//             throw new System.NotImplementedException();
//         }
//     }

//     public class PlatformerCharacterFall : IState
//     {
//         public void OnEnter()
//         {
//             throw new System.NotImplementedException();
//         }

//         public void OnExit()
//         {
//             throw new System.NotImplementedException();
//         }

//         public void OnUpdate(float dt)
//         {
//             throw new System.NotImplementedException();
//         }
//     }

//     public class PlatformerCharacterDead : IState
//     {
//         public void OnEnter()
//         {
//             throw new System.NotImplementedException();
//         }

//         public void OnExit()
//         {
//             throw new System.NotImplementedException();
//         }

//         public void OnUpdate(float dt)
//         {
//             throw new System.NotImplementedException();
//         }
//     }
// }