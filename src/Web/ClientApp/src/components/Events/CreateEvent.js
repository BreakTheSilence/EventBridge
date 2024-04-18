import CreateEventForm from "../Events/CreateEventForm"

function CreateEvent() {

  return (
    <div className="App">
      <header>
        <h1>Event Creation Page</h1>
      </header>
      <main>
        <CreateEventForm/>
      </main>
    </div>
  )
}

export default CreateEvent;