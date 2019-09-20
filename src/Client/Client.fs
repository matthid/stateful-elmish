module Client

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json

open Shared

type Model = { Shared: State.Model; Comp1 : Comp1.Model; Comp2 : Comp2.Model }

type Msg =
    | StateMsg of State.Msg
    | Comp1 of Comp1.Msg
    | Comp2 of Comp2.Msg

let init () : Model * Cmd<Msg> =
    let initShared = State.init()
    let initComp1 = Comp1.init()
    let initComp2 = Comp2.init()
    let initialModel = { Shared = initShared; Comp1 = initComp1; Comp2 = initComp2 }
    initialModel, Cmd.none

let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
    match msg with
    | StateMsg msg ->
        let updateModel, cmd = State.update msg model.Shared
        { model with Shared = updateModel }, Cmd.map StateMsg cmd
    | Comp1 msg ->
        let sharedCmd, mgr = State.StateManager.Create(model.Shared)
        let updateModel, cmd = Comp1.update mgr msg model.Comp1
        // SharedCmd should be first!
        { model with Comp1 = updateModel }, Cmd.batch [ Cmd.map StateMsg sharedCmd; Cmd.map Comp1 cmd  ]
    | Comp2 msg ->
        let sharedCmd, mgr = State.StateManager.Create(model.Shared)
        let updateModel, cmd = Comp2.update mgr msg model.Comp2
        // SharedCmd should be first!
        { model with Comp2 = updateModel }, Cmd.batch [ Cmd.map StateMsg sharedCmd; Cmd.map Comp2 cmd ]
    //| _ -> model, Cmd.none

let safeComponents =
    let components =
        span [ ]
           [ a [ Href "https://github.com/SAFE-Stack/SAFE-template" ]
               [ str "SAFE  "
                 str Version.template ]
             str ", "
             a [ Href "https://saturnframework.github.io" ] [ str "Saturn" ]
             str ", "
             a [ Href "http://fable.io" ] [ str "Fable" ]
             str ", "
             a [ Href "https://elmish.github.io" ] [ str "Elmish" ]
             str ", "
             a [ Href "https://fulma.github.io/Fulma" ] [ str "Fulma" ]

           ]

    span [ ]
        [ str "Version "
          strong [ ] [ str Version.app ]
          str " powered by: "
          components ]


let button txt onClick =
    Button.button
        [ Button.IsFullWidth
          Button.Color IsPrimary
          Button.OnClick onClick ]
        [ str txt ]

let view (model : Model) (dispatch : Msg -> unit) =
    div []
        [ Navbar.navbar [ Navbar.Color IsPrimary ]
            [ Navbar.Item.div [ ]
                [ Heading.h2 [ ]
                    [ str "SAFE Template" ] ] ]


          Footer.footer [ ]
                [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                    [ safeComponents ] ] ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
