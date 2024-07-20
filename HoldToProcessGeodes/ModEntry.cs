// Copyright 2024 Jamie Taylor
namespace HoldToProcessGeodes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardewModdingAPI;
using StardewModdingAPI.Events;

using StardewValley;
using StardewValley.Menus;

public class ModEntry : Mod {
    private ButtonState prevState = ButtonState.Released;
    private bool clickStartedInGeodeSpot = false;

    public override void Entry(IModHelper helper) {
        helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
    }

    private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e) {
        var newState = Game1.input.GetMouseState().LeftButton;
        if (Game1.activeClickableMenu is GeodeMenu menu) {
            if (newState == ButtonState.Pressed) {
                int mouseX = Game1.getMouseX(true);
                int mouseY = Game1.getMouseY(true);
                if (prevState == ButtonState.Released) {
                    // new click; check whether it's in the geode area
                    clickStartedInGeodeSpot = menu.geodeSpot.containsPoint(mouseX, mouseY);
                }
                if (clickStartedInGeodeSpot 
                    && menu.geodeSpot.containsPoint(mouseX, mouseY)
                    && menu.geodeAnimationTimer <= 0
                    && menu.alertTimer <= 0
                    && menu.wiggleWordsTimer <= 0
                    && Game1.dayTimeMoneyBox.moneyShakeTimer <= 0)
                {
                    menu.receiveLeftClick(mouseX, mouseY);
                }
            } else {
                clickStartedInGeodeSpot = false;
            }
        } else {
            clickStartedInGeodeSpot = false;
        }
        prevState = newState;
    }

}
