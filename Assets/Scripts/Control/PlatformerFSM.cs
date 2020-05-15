// using FridgeLogic.Movement;
// using FridgeLogic.Patterns.FSM;
// using UnityEngine;

// namespace FridgeLogic.Control
// {
//     public class PlatformerFSM
//     {
//         private readonly IPlatformerInput _input;
//         private readonly IPlatformerCharacter _character;
//         private readonly StateMachine _stateMachine;

//         public PlatformerFSM(IPlatformerInput input, IPlatformerCharacter character)
//         {
//             _input = input;
//             _character = character;
//             _stateMachine = new StateMachine();

//             InitializeStateMachine();
//         }

//         private void InitializeStateMachine()
//         {
//             var idle = new PlatformerIdle();
//             var walk = new PlatformerWalk();
//             var run = new PlatformerRun();
//             var jump = new PlatformerJump();
//             var fall = new PlatformerFall();
//             var dead = new PlatformerDead();

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
//             );
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
//         Vector2 Velocity { get; set; }
//         float RunThreshold { get; set; }
//         bool IsGrounded { get; set; }
//         bool IsDead { get; set; }
//     }

//     public class PlatformerIdle : IState
//     {
//         public PlatformerIdle(MovementController movementController, JumpController jumpController) : base(movementController, jumpController)
//         {
//         }

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

//     public class PlatformerWalk : IState
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

//     public class PlatformerRun : IState
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

//     public class PlatformerJump : IState
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

//     public class PlatformerFall : IState
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

//     public class PlatformerDead : IState
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