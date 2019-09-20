
module State

open Elmish

// This model 'needs to be updated' from multiple components
type Model = { State: int }

// The Msg type defines what events/actions can occur while the application is running
// the state of the application changes *only* in reaction to these events
type Msg =
    | Increment
    | Decrement

let init () =
    { State = 0 }

let update (msg:Msg) (model:Model) =
    match msg with
    | Increment -> { model with State = model.State + 1 }, Cmd.none
    | Decrement -> { model with State = model.State - 1 }, Cmd.none

type IStateManager =
    abstract State : Model
    abstract Dispatch : Dispatch<Msg>
    abstract StartIncrement : unit -> unit
    abstract StartDecrement : unit -> unit

let createDispatchAndCmd () =
    let mutable buffer = []
    let mutable dispatch = None
    let f msg =
        match dispatch with
        | Some dispatch ->
            dispatch msg
        | None -> buffer <- msg :: buffer
    
    let cmd = Cmd.ofSub(fun d ->
        match dispatch with
        | Some _ -> failwithf "cannot execute this Cmd twice"
        | None ->
            let rev = buffer |> List.rev
            for m in rev do
                d m
            buffer <- []
            dispatch <- Some d
        ())
    f, cmd    

type StateManager (state : Model, dispatch : Dispatch<Msg>) =
    interface IStateManager with
        member x.Dispatch = dispatch
        member x.State = state
        member x.StartIncrement () =
            dispatch Increment
        member x.StartDecrement () =
            dispatch Decrement
    static member Create(state:Model) =
        let dispatch, cmd = createDispatchAndCmd ()
        cmd, StateManager(state, dispatch)

open Elmish.React
open Fable.React
open Fable.React.Props
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json

let button txt onClick =
    Button.button
        [ Button.IsFullWidth
          Button.Color IsPrimary
          Button.OnClick onClick ]
        [ str txt ]

let view (state:Model) (dispatch:Dispatch<Msg>) =
    Container.container []
      [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
            [ Heading.h3 [] [ str ("Press buttons to manipulate counter: " + string state.State) ] ]
        Columns.columns []
            [ Column.column [] [ button "-" (fun _ -> dispatch Decrement) ]
              Column.column [] [ button "+" (fun _ -> dispatch Increment) ] ] ]