using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateGameJam16Weaponizer.Scripts.Interaction
{
    public partial class AxeInteractable : BaseInteractable
    {
        private AnimatedSprite2D sprite;
        private CollisionShape2D collisionShape;

        public override void _Ready()
        {
            base._Ready();
            RequiresSpecificWeapon = false;

            CollisionLayer = 2;
            CollisionMask = 0;

            sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            sprite.Play();
            collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        }

        protected override void PerformInteraction(string weaponType, Vector2 interactionDirection)
        {
            GlobalGameVariables.Instance.PlayerInventory[GlobalGameVariables.Instance.WeaponList.Axe] = true;
            collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
            sprite.Visible = false;
            QueueFree();
            EmitSignal(SignalName.InteractionCompleted);
        }

        public override void _ExitTree()
        {
            base._ExitTree();
        }
    }
}
