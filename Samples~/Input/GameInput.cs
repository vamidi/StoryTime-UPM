// GENERATED AUTOMATICALLY FROM 'Assets/Samples/StoryTime/1.6.3-preview/Input/GameInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace StoryTime.Input
{
    public class @GameInput : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @GameInput()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInput"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""37734faa-64e4-48a4-901b-e0ff80b636be"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""103cf56a-d357-491a-8559-592da00641cc"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Value"",
                    ""id"": ""d603fc26-b424-4cff-a593-71ee48e3d0ef"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""65a85ccc-c121-43d5-b6c9-83674e675ddc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""d0d85176-6fc5-486b-bc5c-36cdd4cb6a54"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""f788c753-f77b-43bf-b5b7-bc98b77814ec"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""OpenInventory"",
                    ""type"": ""Button"",
                    ""id"": ""1b6ae4df-f5e6-42fc-92b6-3dcc2e126563"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""1c4a88b5-2474-4e49-8f10-f987bde2dee0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseControlCamera"",
                    ""type"": ""Button"",
                    ""id"": ""3172ea7f-84a3-4236-b03d-2beae11fa2e0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Ability1"",
                    ""type"": ""Value"",
                    ""id"": ""a64c1d57-e878-40a2-adb2-2afc6e8f9468"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""7268bf2e-283f-47c4-aac9-12e95732868d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad Left Stick"",
                    ""id"": ""bb70cc63-ec03-4bda-b1f6-edc72656ba83"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""39063a94-c3b3-4fd9-8dd6-f53600a700dd"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""49ff05d5-0d8a-4f79-bd99-fd41cdf0d7bd"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1fc52241-074a-41ac-8a81-2ce840b0290b"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""dfd22222-a8e3-4cdb-8f48-6cbffc53def7"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard WASD"",
                    ""id"": ""f8b893c4-85c2-492d-89ab-41f8d1520def"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""896f18e7-b01d-47a3-a018-7c2230b42022"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2247f5b8-f6a1-4a90-89f4-9846090bd18c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""204e90e1-b388-4636-9ba9-6f13df8ba19f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""611822fd-3a0d-49e1-9c2b-960f877aba76"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard Arrows"",
                    ""id"": ""b437a09d-8ca4-4996-a2f1-edf980623f6e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=0.6,y=0.6)"",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d0910d75-c140-4885-b9be-5bb8deae80e0"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""47380ccb-897f-4a93-bac4-3ea860b26105"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""42344a1d-be6c-4a9b-855c-bb33dd318651"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""13a640b3-5350-4478-8218-495187c01f40"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""265ab87e-ba29-4a65-8b54-518f12192e5d"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""645ba428-0631-4e32-b9b0-eaac5d9ed36d"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""95428e26-6672-49d7-b72b-ccbfd5b4267d"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c248131-d3fe-4545-b4dc-ba0690c01312"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8b85e5da-3435-4347-9595-50cc57356b80"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""433e8393-e1b1-484a-af51-d084849fe7c3"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d0e5ef4e-a086-4861-be73-3ab5ea76521f"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cd45c714-729f-438c-bdda-bbfa1bfd66ca"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""937b421c-9467-4b47-a1fd-445b9b44d5c9"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""OpenInventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ceb892d5-04df-4ccf-8ef6-2a5124d4a2e7"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""OpenInventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2b832572-50ef-4dba-aaeb-964ed1c854bb"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone,ScaleVector2(x=8,y=8)"",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard IJKL"",
                    ""id"": ""8e17360e-f036-4963-93f6-05d14787dfb5"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=8,y=8)"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9a4ec3f5-78bf-4f42-b7d7-f1edd7ce12dc"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5d82874e-55d8-4d54-84f5-c4d59dc3b39c"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""13a13bd5-f4ec-4f5b-9a8e-ef1165cc9f5f"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""78626b57-196b-4f24-9a3f-af2477d3633b"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""88e5a69d-cf1d-43ce-b5ab-9db67f819255"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""55713f13-8452-439f-b87e-76d9f2839d7e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MouseControlCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""199fa1b1-e6d3-4539-b663-6ea00c2ba13f"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Ability1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38f581b8-c558-49ee-9f58-17fd3c59d3ae"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Ability1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba5f7b03-c2ca-41c1-9026-6039d202235d"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a3619381-4f1c-4979-9c90-4b17cd15b64f"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menus"",
            ""id"": ""a6b0dc37-2ef3-420d-990a-eef5df26ae21"",
            ""actions"": [
                {
                    ""name"": ""MoveSelection"",
                    ""type"": ""Value"",
                    ""id"": ""ca8fee63-58a3-49c0-9b34-e74351e25113"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""96e1d616-4de6-46d2-9e46-9ba5a4430ff2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""c0e250d4-50a1-4b32-a5e6-f4d8a75bdcd7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseMove"",
                    ""type"": ""PassThrough"",
                    ""id"": ""df62b304-145b-43af-9a21-8cfd14f72fdb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Unpause"",
                    ""type"": ""Button"",
                    ""id"": ""a33f8309-65f3-4f8a-8671-c7cb90ef798e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""66fe9f25-70cd-4276-8872-73b4d1e1a823"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8fee561c-d3a4-460f-98fa-b3c1844f1245"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9efd6cbf-28cf-4873-accc-59f81c4d8704"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""77bc3cae-36d6-4cda-bbf3-5f245a50eca2"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b03f649e-d5ec-4fad-8614-6a69620c5345"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Gamepad Left Stick"",
                    ""id"": ""abfca31e-4360-48f9-89ac-897570e32b3c"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9911cc1d-66fd-4d0c-8c17-bffa1050d574"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4ac5ac47-6988-4dfd-87d2-8da3f9879982"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d07aa007-804a-421e-b73d-90d6f4c352d5"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""eb46138f-9efd-4063-85f9-1f63a70e7d13"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard WASD"",
                    ""id"": ""3d7e43b9-64e0-4b6a-b82e-6eb504598a57"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d351c5e4-c757-4781-af1d-ee5173546559"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""91ab3aad-c9f9-4d1c-be6f-6c4703800e3e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c037d50f-2813-4060-850d-da9525809452"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e95b9649-71dc-4d05-8634-388360c5aee8"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard Arrows"",
                    ""id"": ""502d18cb-7ec6-408f-94c2-51c48f26b95e"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f2cd0574-12ba-4c7d-80ec-94d9646a00e4"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""65171f67-9d60-4a1b-9b0a-0e0db39f1dad"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""57f0f1a3-e2b7-4649-92d0-6336f20b6904"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a3ea2f5c-5bbb-4819-b9e0-64176a9e24fe"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad Dpad"",
                    ""id"": ""7c942501-2d6d-4784-9939-abbe53f3baf9"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b8e01aae-a9c3-4ea9-9ade-8923eca414ff"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7e359d14-3b28-4ec9-836a-d64662f05a0e"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ac3f7d0f-447d-4f07-8b43-5799a109f525"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a4179b2d-1260-409a-97bc-8179322d1f79"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9041691b-5024-463a-b032-59d7f10c38b3"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1decef6d-d60a-486d-a7da-b0d20e2b18b3"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""10a96123-fa2f-43f5-b596-4ed4c93fd4b3"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Unpause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b8857ec1-9a5c-4cd2-af29-276683c9151e"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Unpause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23583bb1-6dd5-4287-94bd-3028199fc088"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;KeyboardOrGamepad"",
                    ""action"": ""MouseMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Dialogues"",
            ""id"": ""53768bd8-c94e-4805-abee-e6c15ca1fa64"",
            ""actions"": [
                {
                    ""name"": ""MoveSelection"",
                    ""type"": ""Value"",
                    ""id"": ""321a676a-1117-48f9-87de-5137dbc7ff36"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AdvanceDialogue"",
                    ""type"": ""Button"",
                    ""id"": ""9ace3f39-c79b-4bbc-ac18-26cafae1f479"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7fdec36e-9182-4c02-b692-3025d78c8431"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""AdvanceDialogue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""08979c15-12c2-4736-962a-03eec0bff52f"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""79fef163-a828-41ee-a93a-f048723efc24"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""AdvanceDialogue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Gamepad Left Stick"",
                    ""id"": ""e394ce70-6a98-45cb-98ee-3b20d27a398a"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8bc6adce-3a54-42ce-8517-eece88cb661f"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5aff7603-0943-4b26-b8cc-d6eac659e263"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad Dpad"",
                    ""id"": ""acc1b4ce-fbf1-42e8-b5bc-adbd2d6683e7"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e3bf74b2-d43d-4388-9799-ea5cfb9a5f0e"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3feb6021-6cda-4c4c-8c3a-5759797818b7"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ff186dd7-80dc-45d5-abed-2a1508fdea05"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ccaa05f6-5ae3-415e-8aca-2979f97ed110"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""434b8802-4e6f-4fd8-b9a3-29259beb030e"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c3b4dc99-5a17-4abf-bd3b-0dff6bee59e5"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard WASD"",
                    ""id"": ""eb995151-9fec-4a96-982d-35630997a2db"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e7ae5426-fddc-4538-8b11-02f3775e8a94"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""67eae0cd-5651-4dd3-ae9a-d77e8b6ccfc0"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""42a182be-4f7a-4b9e-b71f-e40bbfda9d65"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""86e17316-e711-4701-946b-a1dfbccae2dc"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard Arrows"",
                    ""id"": ""96e37e7c-b10b-4cd5-bf7e-fde7c8245ecb"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8fe5410a-1366-4abc-98fd-0eb547c66d6c"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""38e85ec9-d601-4e86-acbe-6f61ff72476a"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""46038b59-6fc5-4cad-b0c3-78728baa916b"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""26ea8a5e-e77d-416c-aa81-28deef164ae9"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardOrGamepad"",
                    ""action"": ""MoveSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardOrGamepad"",
            ""bindingGroup"": ""KeyboardOrGamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Gameplay
            m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
            m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
            m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
            m_Gameplay_Fire = m_Gameplay.FindAction("Fire", throwIfNotFound: true);
            m_Gameplay_Interact = m_Gameplay.FindAction("Interact", throwIfNotFound: true);
            m_Gameplay_Pause = m_Gameplay.FindAction("Pause", throwIfNotFound: true);
            m_Gameplay_OpenInventory = m_Gameplay.FindAction("OpenInventory", throwIfNotFound: true);
            m_Gameplay_Look = m_Gameplay.FindAction("Look", throwIfNotFound: true);
            m_Gameplay_MouseControlCamera = m_Gameplay.FindAction("MouseControlCamera", throwIfNotFound: true);
            m_Gameplay_Ability1 = m_Gameplay.FindAction("Ability1", throwIfNotFound: true);
            m_Gameplay_Aim = m_Gameplay.FindAction("Aim", throwIfNotFound: true);
            // Menus
            m_Menus = asset.FindActionMap("Menus", throwIfNotFound: true);
            m_Menus_MoveSelection = m_Menus.FindAction("MoveSelection", throwIfNotFound: true);
            m_Menus_Confirm = m_Menus.FindAction("Confirm", throwIfNotFound: true);
            m_Menus_Cancel = m_Menus.FindAction("Cancel", throwIfNotFound: true);
            m_Menus_MouseMove = m_Menus.FindAction("MouseMove", throwIfNotFound: true);
            m_Menus_Unpause = m_Menus.FindAction("Unpause", throwIfNotFound: true);
            // Dialogues
            m_Dialogues = asset.FindActionMap("Dialogues", throwIfNotFound: true);
            m_Dialogues_MoveSelection = m_Dialogues.FindAction("MoveSelection", throwIfNotFound: true);
            m_Dialogues_AdvanceDialogue = m_Dialogues.FindAction("AdvanceDialogue", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Gameplay
        private readonly InputActionMap m_Gameplay;
        private IGameplayActions m_GameplayActionsCallbackInterface;
        private readonly InputAction m_Gameplay_Move;
        private readonly InputAction m_Gameplay_Jump;
        private readonly InputAction m_Gameplay_Fire;
        private readonly InputAction m_Gameplay_Interact;
        private readonly InputAction m_Gameplay_Pause;
        private readonly InputAction m_Gameplay_OpenInventory;
        private readonly InputAction m_Gameplay_Look;
        private readonly InputAction m_Gameplay_MouseControlCamera;
        private readonly InputAction m_Gameplay_Ability1;
        private readonly InputAction m_Gameplay_Aim;
        public struct GameplayActions
        {
            private @GameInput m_Wrapper;
            public GameplayActions(@GameInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_Gameplay_Move;
            public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
            public InputAction @Fire => m_Wrapper.m_Gameplay_Fire;
            public InputAction @Interact => m_Wrapper.m_Gameplay_Interact;
            public InputAction @Pause => m_Wrapper.m_Gameplay_Pause;
            public InputAction @OpenInventory => m_Wrapper.m_Gameplay_OpenInventory;
            public InputAction @Look => m_Wrapper.m_Gameplay_Look;
            public InputAction @MouseControlCamera => m_Wrapper.m_Gameplay_MouseControlCamera;
            public InputAction @Ability1 => m_Wrapper.m_Gameplay_Ability1;
            public InputAction @Aim => m_Wrapper.m_Gameplay_Aim;
            public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
            public void SetCallbacks(IGameplayActions instance)
            {
                if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
                {
                    @Move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                    @Jump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                    @Jump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                    @Jump.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                    @Fire.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFire;
                    @Fire.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFire;
                    @Fire.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFire;
                    @Interact.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                    @Interact.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                    @Interact.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                    @Pause.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                    @Pause.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                    @Pause.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                    @OpenInventory.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnOpenInventory;
                    @OpenInventory.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnOpenInventory;
                    @OpenInventory.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnOpenInventory;
                    @Look.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                    @Look.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                    @Look.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                    @MouseControlCamera.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouseControlCamera;
                    @MouseControlCamera.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouseControlCamera;
                    @MouseControlCamera.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMouseControlCamera;
                    @Ability1.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbility1;
                    @Ability1.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbility1;
                    @Ability1.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbility1;
                    @Aim.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAim;
                    @Aim.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAim;
                    @Aim.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAim;
                }
                m_Wrapper.m_GameplayActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @Jump.started += instance.OnJump;
                    @Jump.performed += instance.OnJump;
                    @Jump.canceled += instance.OnJump;
                    @Fire.started += instance.OnFire;
                    @Fire.performed += instance.OnFire;
                    @Fire.canceled += instance.OnFire;
                    @Interact.started += instance.OnInteract;
                    @Interact.performed += instance.OnInteract;
                    @Interact.canceled += instance.OnInteract;
                    @Pause.started += instance.OnPause;
                    @Pause.performed += instance.OnPause;
                    @Pause.canceled += instance.OnPause;
                    @OpenInventory.started += instance.OnOpenInventory;
                    @OpenInventory.performed += instance.OnOpenInventory;
                    @OpenInventory.canceled += instance.OnOpenInventory;
                    @Look.started += instance.OnLook;
                    @Look.performed += instance.OnLook;
                    @Look.canceled += instance.OnLook;
                    @MouseControlCamera.started += instance.OnMouseControlCamera;
                    @MouseControlCamera.performed += instance.OnMouseControlCamera;
                    @MouseControlCamera.canceled += instance.OnMouseControlCamera;
                    @Ability1.started += instance.OnAbility1;
                    @Ability1.performed += instance.OnAbility1;
                    @Ability1.canceled += instance.OnAbility1;
                    @Aim.started += instance.OnAim;
                    @Aim.performed += instance.OnAim;
                    @Aim.canceled += instance.OnAim;
                }
            }
        }
        public GameplayActions @Gameplay => new GameplayActions(this);

        // Menus
        private readonly InputActionMap m_Menus;
        private IMenusActions m_MenusActionsCallbackInterface;
        private readonly InputAction m_Menus_MoveSelection;
        private readonly InputAction m_Menus_Confirm;
        private readonly InputAction m_Menus_Cancel;
        private readonly InputAction m_Menus_MouseMove;
        private readonly InputAction m_Menus_Unpause;
        public struct MenusActions
        {
            private @GameInput m_Wrapper;
            public MenusActions(@GameInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @MoveSelection => m_Wrapper.m_Menus_MoveSelection;
            public InputAction @Confirm => m_Wrapper.m_Menus_Confirm;
            public InputAction @Cancel => m_Wrapper.m_Menus_Cancel;
            public InputAction @MouseMove => m_Wrapper.m_Menus_MouseMove;
            public InputAction @Unpause => m_Wrapper.m_Menus_Unpause;
            public InputActionMap Get() { return m_Wrapper.m_Menus; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MenusActions set) { return set.Get(); }
            public void SetCallbacks(IMenusActions instance)
            {
                if (m_Wrapper.m_MenusActionsCallbackInterface != null)
                {
                    @MoveSelection.started -= m_Wrapper.m_MenusActionsCallbackInterface.OnMoveSelection;
                    @MoveSelection.performed -= m_Wrapper.m_MenusActionsCallbackInterface.OnMoveSelection;
                    @MoveSelection.canceled -= m_Wrapper.m_MenusActionsCallbackInterface.OnMoveSelection;
                    @Confirm.started -= m_Wrapper.m_MenusActionsCallbackInterface.OnConfirm;
                    @Confirm.performed -= m_Wrapper.m_MenusActionsCallbackInterface.OnConfirm;
                    @Confirm.canceled -= m_Wrapper.m_MenusActionsCallbackInterface.OnConfirm;
                    @Cancel.started -= m_Wrapper.m_MenusActionsCallbackInterface.OnCancel;
                    @Cancel.performed -= m_Wrapper.m_MenusActionsCallbackInterface.OnCancel;
                    @Cancel.canceled -= m_Wrapper.m_MenusActionsCallbackInterface.OnCancel;
                    @MouseMove.started -= m_Wrapper.m_MenusActionsCallbackInterface.OnMouseMove;
                    @MouseMove.performed -= m_Wrapper.m_MenusActionsCallbackInterface.OnMouseMove;
                    @MouseMove.canceled -= m_Wrapper.m_MenusActionsCallbackInterface.OnMouseMove;
                    @Unpause.started -= m_Wrapper.m_MenusActionsCallbackInterface.OnUnpause;
                    @Unpause.performed -= m_Wrapper.m_MenusActionsCallbackInterface.OnUnpause;
                    @Unpause.canceled -= m_Wrapper.m_MenusActionsCallbackInterface.OnUnpause;
                }
                m_Wrapper.m_MenusActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @MoveSelection.started += instance.OnMoveSelection;
                    @MoveSelection.performed += instance.OnMoveSelection;
                    @MoveSelection.canceled += instance.OnMoveSelection;
                    @Confirm.started += instance.OnConfirm;
                    @Confirm.performed += instance.OnConfirm;
                    @Confirm.canceled += instance.OnConfirm;
                    @Cancel.started += instance.OnCancel;
                    @Cancel.performed += instance.OnCancel;
                    @Cancel.canceled += instance.OnCancel;
                    @MouseMove.started += instance.OnMouseMove;
                    @MouseMove.performed += instance.OnMouseMove;
                    @MouseMove.canceled += instance.OnMouseMove;
                    @Unpause.started += instance.OnUnpause;
                    @Unpause.performed += instance.OnUnpause;
                    @Unpause.canceled += instance.OnUnpause;
                }
            }
        }
        public MenusActions @Menus => new MenusActions(this);

        // Dialogues
        private readonly InputActionMap m_Dialogues;
        private IDialoguesActions m_DialoguesActionsCallbackInterface;
        private readonly InputAction m_Dialogues_MoveSelection;
        private readonly InputAction m_Dialogues_AdvanceDialogue;
        public struct DialoguesActions
        {
            private @GameInput m_Wrapper;
            public DialoguesActions(@GameInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @MoveSelection => m_Wrapper.m_Dialogues_MoveSelection;
            public InputAction @AdvanceDialogue => m_Wrapper.m_Dialogues_AdvanceDialogue;
            public InputActionMap Get() { return m_Wrapper.m_Dialogues; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(DialoguesActions set) { return set.Get(); }
            public void SetCallbacks(IDialoguesActions instance)
            {
                if (m_Wrapper.m_DialoguesActionsCallbackInterface != null)
                {
                    @MoveSelection.started -= m_Wrapper.m_DialoguesActionsCallbackInterface.OnMoveSelection;
                    @MoveSelection.performed -= m_Wrapper.m_DialoguesActionsCallbackInterface.OnMoveSelection;
                    @MoveSelection.canceled -= m_Wrapper.m_DialoguesActionsCallbackInterface.OnMoveSelection;
                    @AdvanceDialogue.started -= m_Wrapper.m_DialoguesActionsCallbackInterface.OnAdvanceDialogue;
                    @AdvanceDialogue.performed -= m_Wrapper.m_DialoguesActionsCallbackInterface.OnAdvanceDialogue;
                    @AdvanceDialogue.canceled -= m_Wrapper.m_DialoguesActionsCallbackInterface.OnAdvanceDialogue;
                }
                m_Wrapper.m_DialoguesActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @MoveSelection.started += instance.OnMoveSelection;
                    @MoveSelection.performed += instance.OnMoveSelection;
                    @MoveSelection.canceled += instance.OnMoveSelection;
                    @AdvanceDialogue.started += instance.OnAdvanceDialogue;
                    @AdvanceDialogue.performed += instance.OnAdvanceDialogue;
                    @AdvanceDialogue.canceled += instance.OnAdvanceDialogue;
                }
            }
        }
        public DialoguesActions @Dialogues => new DialoguesActions(this);
        private int m_KeyboardOrGamepadSchemeIndex = -1;
        public InputControlScheme KeyboardOrGamepadScheme
        {
            get
            {
                if (m_KeyboardOrGamepadSchemeIndex == -1) m_KeyboardOrGamepadSchemeIndex = asset.FindControlSchemeIndex("KeyboardOrGamepad");
                return asset.controlSchemes[m_KeyboardOrGamepadSchemeIndex];
            }
        }
        public interface IGameplayActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
            void OnFire(InputAction.CallbackContext context);
            void OnInteract(InputAction.CallbackContext context);
            void OnPause(InputAction.CallbackContext context);
            void OnOpenInventory(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
            void OnMouseControlCamera(InputAction.CallbackContext context);
            void OnAbility1(InputAction.CallbackContext context);
            void OnAim(InputAction.CallbackContext context);
        }
        public interface IMenusActions
        {
            void OnMoveSelection(InputAction.CallbackContext context);
            void OnConfirm(InputAction.CallbackContext context);
            void OnCancel(InputAction.CallbackContext context);
            void OnMouseMove(InputAction.CallbackContext context);
            void OnUnpause(InputAction.CallbackContext context);
        }
        public interface IDialoguesActions
        {
            void OnMoveSelection(InputAction.CallbackContext context);
            void OnAdvanceDialogue(InputAction.CallbackContext context);
        }
    }
}
