module Comp2

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
    model, Cmd.none

