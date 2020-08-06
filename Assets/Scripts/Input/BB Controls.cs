// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input/BB Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @BBControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @BBControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""BB Controls"",
    ""maps"": [
        {
            ""name"": ""Main"",
            ""id"": ""226159b9-02bc-4693-b121-c1fc6fa0f656"",
            ""actions"": [
                {
                    ""name"": ""Camera XZ"",
                    ""type"": ""Value"",
                    ""id"": ""f1df6f0c-f78e-4dab-aaa8-714ad89609ee"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera Y"",
                    ""type"": ""Value"",
                    ""id"": ""112acdb1-4342-4d19-91e5-c4f979700e34"",
                    ""expectedControlType"": ""Integer"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Speed"",
                    ""type"": ""Value"",
                    ""id"": ""55634b6c-b581-4bab-9218-03f6d16f10dc"",
                    ""expectedControlType"": ""Digital"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera Look"",
                    ""type"": ""Value"",
                    ""id"": ""54dcc725-100d-4fca-93df-e7876fb02aed"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""ScaleVector2(x=0.5,y=0.5),ScaleVector2(x=0.1,y=0.1)"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Selection X"",
                    ""type"": ""Button"",
                    ""id"": ""4ee4859a-cb80-4e0e-bce1-ca28371707b9"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Selection Y"",
                    ""type"": ""Button"",
                    ""id"": ""685f9745-b165-4be5-a07d-5b13d6f34d6f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Selection Z"",
                    ""type"": ""Button"",
                    ""id"": ""b935a941-02f8-4392-9e65-d9625a8a2747"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""d59c169b-073c-4cd0-bdf6-d4a8eddeedd8"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera XZ"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8fa20a4a-9daa-404b-900c-726808ed12dc"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera XZ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6a83f278-04c4-4ed6-b457-1f95a846aadd"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera XZ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f8f4496b-9d52-4c78-8b02-885d7b21731e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera XZ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1a17c1f0-1f60-42d3-9e49-350e438c836e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera XZ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""QE"",
                    ""id"": ""217d2436-9fee-4f5e-b394-8ab96e221042"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera Y"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""18cee702-af74-4b78-926e-53d63c72a01f"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera Y"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""9dcc57dc-0c57-4416-9e0e-76ffeee58d11"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera Y"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""46c12ada-cf0d-494d-8744-70d71709514f"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false)"",
                    ""groups"": """",
                    ""action"": ""Camera Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""44bc123e-df04-40f8-8aa1-078409cb891c"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Speed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keypad"",
                    ""id"": ""f49aad25-fac2-4805-9bfe-d0990cdd5052"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection X"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""3293aa41-b58a-41d3-a10f-0af7b36f6f1a"",
                    ""path"": ""<Keyboard>/numpad4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection X"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""08334ffd-7c8f-4825-9220-85b27d781bdc"",
                    ""path"": ""<Keyboard>/numpad6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection X"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""91ff503e-e73d-45c7-b056-960426101d6b"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection Y"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""6b204a8f-6ce9-4e3f-af32-e75864a702e0"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection Y"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""3704e9ef-e5c4-4319-b75d-14cfb9bffd87"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection Y"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keypad"",
                    ""id"": ""8beb084e-1d6d-4fe4-bc71-22fe193bb8f3"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection Z"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""1fcafbf0-5d52-4d21-813f-672f4e0a6322"",
                    ""path"": ""<Keyboard>/numpad2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection Z"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""34f3be1d-9e2d-4752-9c7e-710742e572c3"",
                    ""path"": ""<Keyboard>/numpad8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection Z"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Editor Keys"",
            ""id"": ""bdebe45a-4381-4588-a228-69a242873c4f"",
            ""actions"": [
                {
                    ""name"": ""Control"",
                    ""type"": ""Button"",
                    ""id"": ""7f59ea11-cce7-4a62-9b6b-32ea206cc001"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""New Brick"",
                    ""type"": ""Button"",
                    ""id"": ""38f2136c-00b3-41e5-8c14-2f77974db200"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Copy"",
                    ""type"": ""Button"",
                    ""id"": ""824d0ca6-a3cb-4d05-accb-30b23b3f2663"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Paste"",
                    ""type"": ""Button"",
                    ""id"": ""f2035bec-d948-4021-b887-8b419532b167"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cut"",
                    ""type"": ""Button"",
                    ""id"": ""ab963f0c-a131-4c6a-ad19-dbf11642d469"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Duplicate"",
                    ""type"": ""Button"",
                    ""id"": ""2eb64c73-7a56-494a-b49e-e95c9cc32571"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Save File"",
                    ""type"": ""Button"",
                    ""id"": ""e9746926-a66c-4fef-9c06-cd717027d97c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Open File"",
                    ""type"": ""Button"",
                    ""id"": ""5737601f-7ef7-4546-8dac-aebe2726b750"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select All"",
                    ""type"": ""Button"",
                    ""id"": ""4729160c-def9-4e14-b4d8-29d032552d15"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Delete Brick"",
                    ""type"": ""Button"",
                    ""id"": ""56d0e22d-d67f-45ba-b46e-3ba038800365"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""6dbb9afb-a746-43ae-a600-b1ea8ce91ed8"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Translate"",
                    ""type"": ""Button"",
                    ""id"": ""c589892b-0d95-4390-8cbe-e3119ef5131a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Scale"",
                    ""type"": ""Button"",
                    ""id"": ""4a718206-4aa9-4960-b50b-437b80a673c7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Button"",
                    ""id"": ""c02bfcc7-4bd9-43c6-a7a8-0003e6b1a74a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Undo"",
                    ""type"": ""Button"",
                    ""id"": ""2ce3de83-ee6e-449d-acf7-41f353e5ed33"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Redo"",
                    ""type"": ""Button"",
                    ""id"": ""33423e5b-888c-40e1-b2c5-8dfd75aa76cb"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Group Selection"",
                    ""type"": ""Button"",
                    ""id"": ""cc3c0572-f4b5-456a-879f-e6cb764034bb"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RebindTemp"",
                    ""type"": ""Button"",
                    ""id"": ""98ceb0c1-adeb-44ba-8041-a963e72e257b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""66e8f022-f16b-4d10-aeef-745009d30ba4"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New Brick"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""92210945-10d1-40ea-b1fb-a1ec1a55fc7e"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New Brick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""cab183d7-6386-459b-9dcd-5e6493a965c1"",
                    ""path"": ""<Keyboard>/n"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New Brick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""1ab017c8-a4c0-40e6-bfb4-4578a575e715"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Copy"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""5d80a7af-2d4b-42e4-8fe9-44db00f75b5d"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Copy"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""0da31e99-e8c1-4b6a-b7a3-fd0cf57ff8de"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Copy"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""b6063254-6f48-4b73-a4b7-9e981b3b6abb"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Paste"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""45be9efb-8f04-430f-bf68-e27f37b03ee4"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Paste"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""b9acd76e-b832-4ebb-a8b1-9e6f427c05e1"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Paste"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""9d067b40-792f-46a0-8045-d28d1bd6cad5"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Duplicate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""4e34041c-f875-42f6-85b9-98a7738fbb33"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Duplicate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""c97aaa82-aa7d-4d9a-bac0-2dbcb1b913bd"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Duplicate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""ce98e9ca-e6a5-4fa0-8a5f-8779759058e5"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Save File"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""3323a6d5-1f75-4f56-91cc-f3d1f198a955"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Save File"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""a37bb714-deaf-4dab-980c-3a95763443da"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Save File"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""f3a96ecc-3829-412e-a084-6a9673858499"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select All"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""ace566eb-ef26-4657-8386-91e366777fc6"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select All"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""67a69116-2e38-4db1-8ef0-d0e0d0e0dc22"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select All"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""f022aed5-2f72-4596-899f-eb480779c1ca"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Open File"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""acf66b99-a23f-4af0-b5e0-00d9591fd733"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Open File"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""03a82418-aa76-49b0-9188-11efaf900da0"",
                    ""path"": ""<Keyboard>/o"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Open File"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9e12bea1-0505-4d10-9db2-e93c92d113eb"",
                    ""path"": ""<Keyboard>/delete"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Delete Brick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""332f1ca0-9efd-40ba-a839-013e6edb5365"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2da7d94b-c9f0-4bd6-aff7-010d55c78266"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""c5b7291f-3fc1-4bd1-93cc-6c2c6a741b94"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cut"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""4b621c12-02d2-45bd-b871-e936e574a19a"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cut"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""b6aef4f8-2c63-4d71-b44b-cebad95d7215"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cut"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""e1b87db8-11cc-4f7d-b4be-3908cb628ab7"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cut"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""6e3431f4-c4cb-42f2-b529-5053b65cc3f0"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cut"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""df2ddb71-9bbe-4ed4-978a-a1b506075812"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cut"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""80a44e00-906a-43bf-964a-9ec4e496676c"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RebindTemp"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""81cb5b3d-416c-432b-a900-a926c0cd4958"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RebindTemp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""7e22b2b9-2c3d-47a3-9d9e-3fc3dc83bdd0"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RebindTemp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""e27bbd1f-2d8c-43ba-9866-53543b9efad6"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Undo"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""d1335c3b-57a6-4e95-9841-efd723991617"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Undo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""bdcc4a8c-834f-4edd-9ad9-9614c89ff7cf"",
                    ""path"": ""<Keyboard>/Z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Undo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""8109bf42-fde9-4abc-908c-c365c22ba43c"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Redo"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""96de9a4f-c50f-4e80-a054-8593e5afd77a"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Redo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""00ca5491-4637-4ffc-8dc0-3e30de7521b1"",
                    ""path"": ""<Keyboard>/Y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Redo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""fe819817-6a53-470b-8e85-982e4e2e96df"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Translate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a18d9e3c-46e8-47ba-962e-a8ceabd53631"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ad8826b-6079-4e18-ac37-6631a779cf5f"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scale"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""85c20f48-0ac4-4df2-8a05-6b86acdd0b69"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Group Selection"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""629b16a1-3d88-40c7-ba90-553da5fae752"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Group Selection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""efd0b5cb-f82d-4f93-9d5f-bfa23277d912"",
                    ""path"": ""<Keyboard>/G"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Group Selection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Main
        m_Main = asset.FindActionMap("Main", throwIfNotFound: true);
        m_Main_CameraXZ = m_Main.FindAction("Camera XZ", throwIfNotFound: true);
        m_Main_CameraY = m_Main.FindAction("Camera Y", throwIfNotFound: true);
        m_Main_Speed = m_Main.FindAction("Speed", throwIfNotFound: true);
        m_Main_CameraLook = m_Main.FindAction("Camera Look", throwIfNotFound: true);
        m_Main_SelectionX = m_Main.FindAction("Selection X", throwIfNotFound: true);
        m_Main_SelectionY = m_Main.FindAction("Selection Y", throwIfNotFound: true);
        m_Main_SelectionZ = m_Main.FindAction("Selection Z", throwIfNotFound: true);
        // Editor Keys
        m_EditorKeys = asset.FindActionMap("Editor Keys", throwIfNotFound: true);
        m_EditorKeys_Control = m_EditorKeys.FindAction("Control", throwIfNotFound: true);
        m_EditorKeys_NewBrick = m_EditorKeys.FindAction("New Brick", throwIfNotFound: true);
        m_EditorKeys_Copy = m_EditorKeys.FindAction("Copy", throwIfNotFound: true);
        m_EditorKeys_Paste = m_EditorKeys.FindAction("Paste", throwIfNotFound: true);
        m_EditorKeys_Cut = m_EditorKeys.FindAction("Cut", throwIfNotFound: true);
        m_EditorKeys_Duplicate = m_EditorKeys.FindAction("Duplicate", throwIfNotFound: true);
        m_EditorKeys_SaveFile = m_EditorKeys.FindAction("Save File", throwIfNotFound: true);
        m_EditorKeys_OpenFile = m_EditorKeys.FindAction("Open File", throwIfNotFound: true);
        m_EditorKeys_SelectAll = m_EditorKeys.FindAction("Select All", throwIfNotFound: true);
        m_EditorKeys_DeleteBrick = m_EditorKeys.FindAction("Delete Brick", throwIfNotFound: true);
        m_EditorKeys_Cancel = m_EditorKeys.FindAction("Cancel", throwIfNotFound: true);
        m_EditorKeys_Translate = m_EditorKeys.FindAction("Translate", throwIfNotFound: true);
        m_EditorKeys_Scale = m_EditorKeys.FindAction("Scale", throwIfNotFound: true);
        m_EditorKeys_Rotate = m_EditorKeys.FindAction("Rotate", throwIfNotFound: true);
        m_EditorKeys_Undo = m_EditorKeys.FindAction("Undo", throwIfNotFound: true);
        m_EditorKeys_Redo = m_EditorKeys.FindAction("Redo", throwIfNotFound: true);
        m_EditorKeys_GroupSelection = m_EditorKeys.FindAction("Group Selection", throwIfNotFound: true);
        m_EditorKeys_RebindTemp = m_EditorKeys.FindAction("RebindTemp", throwIfNotFound: true);
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

    // Main
    private readonly InputActionMap m_Main;
    private IMainActions m_MainActionsCallbackInterface;
    private readonly InputAction m_Main_CameraXZ;
    private readonly InputAction m_Main_CameraY;
    private readonly InputAction m_Main_Speed;
    private readonly InputAction m_Main_CameraLook;
    private readonly InputAction m_Main_SelectionX;
    private readonly InputAction m_Main_SelectionY;
    private readonly InputAction m_Main_SelectionZ;
    public struct MainActions
    {
        private @BBControls m_Wrapper;
        public MainActions(@BBControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @CameraXZ => m_Wrapper.m_Main_CameraXZ;
        public InputAction @CameraY => m_Wrapper.m_Main_CameraY;
        public InputAction @Speed => m_Wrapper.m_Main_Speed;
        public InputAction @CameraLook => m_Wrapper.m_Main_CameraLook;
        public InputAction @SelectionX => m_Wrapper.m_Main_SelectionX;
        public InputAction @SelectionY => m_Wrapper.m_Main_SelectionY;
        public InputAction @SelectionZ => m_Wrapper.m_Main_SelectionZ;
        public InputActionMap Get() { return m_Wrapper.m_Main; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainActions set) { return set.Get(); }
        public void SetCallbacks(IMainActions instance)
        {
            if (m_Wrapper.m_MainActionsCallbackInterface != null)
            {
                @CameraXZ.started -= m_Wrapper.m_MainActionsCallbackInterface.OnCameraXZ;
                @CameraXZ.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnCameraXZ;
                @CameraXZ.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnCameraXZ;
                @CameraY.started -= m_Wrapper.m_MainActionsCallbackInterface.OnCameraY;
                @CameraY.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnCameraY;
                @CameraY.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnCameraY;
                @Speed.started -= m_Wrapper.m_MainActionsCallbackInterface.OnSpeed;
                @Speed.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnSpeed;
                @Speed.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnSpeed;
                @CameraLook.started -= m_Wrapper.m_MainActionsCallbackInterface.OnCameraLook;
                @CameraLook.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnCameraLook;
                @CameraLook.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnCameraLook;
                @SelectionX.started -= m_Wrapper.m_MainActionsCallbackInterface.OnSelectionX;
                @SelectionX.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnSelectionX;
                @SelectionX.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnSelectionX;
                @SelectionY.started -= m_Wrapper.m_MainActionsCallbackInterface.OnSelectionY;
                @SelectionY.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnSelectionY;
                @SelectionY.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnSelectionY;
                @SelectionZ.started -= m_Wrapper.m_MainActionsCallbackInterface.OnSelectionZ;
                @SelectionZ.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnSelectionZ;
                @SelectionZ.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnSelectionZ;
            }
            m_Wrapper.m_MainActionsCallbackInterface = instance;
            if (instance != null)
            {
                @CameraXZ.started += instance.OnCameraXZ;
                @CameraXZ.performed += instance.OnCameraXZ;
                @CameraXZ.canceled += instance.OnCameraXZ;
                @CameraY.started += instance.OnCameraY;
                @CameraY.performed += instance.OnCameraY;
                @CameraY.canceled += instance.OnCameraY;
                @Speed.started += instance.OnSpeed;
                @Speed.performed += instance.OnSpeed;
                @Speed.canceled += instance.OnSpeed;
                @CameraLook.started += instance.OnCameraLook;
                @CameraLook.performed += instance.OnCameraLook;
                @CameraLook.canceled += instance.OnCameraLook;
                @SelectionX.started += instance.OnSelectionX;
                @SelectionX.performed += instance.OnSelectionX;
                @SelectionX.canceled += instance.OnSelectionX;
                @SelectionY.started += instance.OnSelectionY;
                @SelectionY.performed += instance.OnSelectionY;
                @SelectionY.canceled += instance.OnSelectionY;
                @SelectionZ.started += instance.OnSelectionZ;
                @SelectionZ.performed += instance.OnSelectionZ;
                @SelectionZ.canceled += instance.OnSelectionZ;
            }
        }
    }
    public MainActions @Main => new MainActions(this);

    // Editor Keys
    private readonly InputActionMap m_EditorKeys;
    private IEditorKeysActions m_EditorKeysActionsCallbackInterface;
    private readonly InputAction m_EditorKeys_Control;
    private readonly InputAction m_EditorKeys_NewBrick;
    private readonly InputAction m_EditorKeys_Copy;
    private readonly InputAction m_EditorKeys_Paste;
    private readonly InputAction m_EditorKeys_Cut;
    private readonly InputAction m_EditorKeys_Duplicate;
    private readonly InputAction m_EditorKeys_SaveFile;
    private readonly InputAction m_EditorKeys_OpenFile;
    private readonly InputAction m_EditorKeys_SelectAll;
    private readonly InputAction m_EditorKeys_DeleteBrick;
    private readonly InputAction m_EditorKeys_Cancel;
    private readonly InputAction m_EditorKeys_Translate;
    private readonly InputAction m_EditorKeys_Scale;
    private readonly InputAction m_EditorKeys_Rotate;
    private readonly InputAction m_EditorKeys_Undo;
    private readonly InputAction m_EditorKeys_Redo;
    private readonly InputAction m_EditorKeys_GroupSelection;
    private readonly InputAction m_EditorKeys_RebindTemp;
    public struct EditorKeysActions
    {
        private @BBControls m_Wrapper;
        public EditorKeysActions(@BBControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Control => m_Wrapper.m_EditorKeys_Control;
        public InputAction @NewBrick => m_Wrapper.m_EditorKeys_NewBrick;
        public InputAction @Copy => m_Wrapper.m_EditorKeys_Copy;
        public InputAction @Paste => m_Wrapper.m_EditorKeys_Paste;
        public InputAction @Cut => m_Wrapper.m_EditorKeys_Cut;
        public InputAction @Duplicate => m_Wrapper.m_EditorKeys_Duplicate;
        public InputAction @SaveFile => m_Wrapper.m_EditorKeys_SaveFile;
        public InputAction @OpenFile => m_Wrapper.m_EditorKeys_OpenFile;
        public InputAction @SelectAll => m_Wrapper.m_EditorKeys_SelectAll;
        public InputAction @DeleteBrick => m_Wrapper.m_EditorKeys_DeleteBrick;
        public InputAction @Cancel => m_Wrapper.m_EditorKeys_Cancel;
        public InputAction @Translate => m_Wrapper.m_EditorKeys_Translate;
        public InputAction @Scale => m_Wrapper.m_EditorKeys_Scale;
        public InputAction @Rotate => m_Wrapper.m_EditorKeys_Rotate;
        public InputAction @Undo => m_Wrapper.m_EditorKeys_Undo;
        public InputAction @Redo => m_Wrapper.m_EditorKeys_Redo;
        public InputAction @GroupSelection => m_Wrapper.m_EditorKeys_GroupSelection;
        public InputAction @RebindTemp => m_Wrapper.m_EditorKeys_RebindTemp;
        public InputActionMap Get() { return m_Wrapper.m_EditorKeys; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(EditorKeysActions set) { return set.Get(); }
        public void SetCallbacks(IEditorKeysActions instance)
        {
            if (m_Wrapper.m_EditorKeysActionsCallbackInterface != null)
            {
                @Control.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnControl;
                @Control.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnControl;
                @Control.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnControl;
                @NewBrick.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnNewBrick;
                @NewBrick.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnNewBrick;
                @NewBrick.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnNewBrick;
                @Copy.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnCopy;
                @Copy.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnCopy;
                @Copy.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnCopy;
                @Paste.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnPaste;
                @Paste.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnPaste;
                @Paste.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnPaste;
                @Cut.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnCut;
                @Cut.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnCut;
                @Cut.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnCut;
                @Duplicate.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnDuplicate;
                @Duplicate.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnDuplicate;
                @Duplicate.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnDuplicate;
                @SaveFile.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnSaveFile;
                @SaveFile.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnSaveFile;
                @SaveFile.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnSaveFile;
                @OpenFile.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnOpenFile;
                @OpenFile.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnOpenFile;
                @OpenFile.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnOpenFile;
                @SelectAll.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnSelectAll;
                @SelectAll.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnSelectAll;
                @SelectAll.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnSelectAll;
                @DeleteBrick.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnDeleteBrick;
                @DeleteBrick.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnDeleteBrick;
                @DeleteBrick.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnDeleteBrick;
                @Cancel.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnCancel;
                @Translate.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnTranslate;
                @Translate.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnTranslate;
                @Translate.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnTranslate;
                @Scale.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnScale;
                @Scale.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnScale;
                @Scale.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnScale;
                @Rotate.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnRotate;
                @Undo.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnUndo;
                @Undo.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnUndo;
                @Undo.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnUndo;
                @Redo.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnRedo;
                @Redo.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnRedo;
                @Redo.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnRedo;
                @GroupSelection.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnGroupSelection;
                @GroupSelection.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnGroupSelection;
                @GroupSelection.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnGroupSelection;
                @RebindTemp.started -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnRebindTemp;
                @RebindTemp.performed -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnRebindTemp;
                @RebindTemp.canceled -= m_Wrapper.m_EditorKeysActionsCallbackInterface.OnRebindTemp;
            }
            m_Wrapper.m_EditorKeysActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Control.started += instance.OnControl;
                @Control.performed += instance.OnControl;
                @Control.canceled += instance.OnControl;
                @NewBrick.started += instance.OnNewBrick;
                @NewBrick.performed += instance.OnNewBrick;
                @NewBrick.canceled += instance.OnNewBrick;
                @Copy.started += instance.OnCopy;
                @Copy.performed += instance.OnCopy;
                @Copy.canceled += instance.OnCopy;
                @Paste.started += instance.OnPaste;
                @Paste.performed += instance.OnPaste;
                @Paste.canceled += instance.OnPaste;
                @Cut.started += instance.OnCut;
                @Cut.performed += instance.OnCut;
                @Cut.canceled += instance.OnCut;
                @Duplicate.started += instance.OnDuplicate;
                @Duplicate.performed += instance.OnDuplicate;
                @Duplicate.canceled += instance.OnDuplicate;
                @SaveFile.started += instance.OnSaveFile;
                @SaveFile.performed += instance.OnSaveFile;
                @SaveFile.canceled += instance.OnSaveFile;
                @OpenFile.started += instance.OnOpenFile;
                @OpenFile.performed += instance.OnOpenFile;
                @OpenFile.canceled += instance.OnOpenFile;
                @SelectAll.started += instance.OnSelectAll;
                @SelectAll.performed += instance.OnSelectAll;
                @SelectAll.canceled += instance.OnSelectAll;
                @DeleteBrick.started += instance.OnDeleteBrick;
                @DeleteBrick.performed += instance.OnDeleteBrick;
                @DeleteBrick.canceled += instance.OnDeleteBrick;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Translate.started += instance.OnTranslate;
                @Translate.performed += instance.OnTranslate;
                @Translate.canceled += instance.OnTranslate;
                @Scale.started += instance.OnScale;
                @Scale.performed += instance.OnScale;
                @Scale.canceled += instance.OnScale;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @Undo.started += instance.OnUndo;
                @Undo.performed += instance.OnUndo;
                @Undo.canceled += instance.OnUndo;
                @Redo.started += instance.OnRedo;
                @Redo.performed += instance.OnRedo;
                @Redo.canceled += instance.OnRedo;
                @GroupSelection.started += instance.OnGroupSelection;
                @GroupSelection.performed += instance.OnGroupSelection;
                @GroupSelection.canceled += instance.OnGroupSelection;
                @RebindTemp.started += instance.OnRebindTemp;
                @RebindTemp.performed += instance.OnRebindTemp;
                @RebindTemp.canceled += instance.OnRebindTemp;
            }
        }
    }
    public EditorKeysActions @EditorKeys => new EditorKeysActions(this);
    public interface IMainActions
    {
        void OnCameraXZ(InputAction.CallbackContext context);
        void OnCameraY(InputAction.CallbackContext context);
        void OnSpeed(InputAction.CallbackContext context);
        void OnCameraLook(InputAction.CallbackContext context);
        void OnSelectionX(InputAction.CallbackContext context);
        void OnSelectionY(InputAction.CallbackContext context);
        void OnSelectionZ(InputAction.CallbackContext context);
    }
    public interface IEditorKeysActions
    {
        void OnControl(InputAction.CallbackContext context);
        void OnNewBrick(InputAction.CallbackContext context);
        void OnCopy(InputAction.CallbackContext context);
        void OnPaste(InputAction.CallbackContext context);
        void OnCut(InputAction.CallbackContext context);
        void OnDuplicate(InputAction.CallbackContext context);
        void OnSaveFile(InputAction.CallbackContext context);
        void OnOpenFile(InputAction.CallbackContext context);
        void OnSelectAll(InputAction.CallbackContext context);
        void OnDeleteBrick(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnTranslate(InputAction.CallbackContext context);
        void OnScale(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnUndo(InputAction.CallbackContext context);
        void OnRedo(InputAction.CallbackContext context);
        void OnGroupSelection(InputAction.CallbackContext context);
        void OnRebindTemp(InputAction.CallbackContext context);
    }
}
