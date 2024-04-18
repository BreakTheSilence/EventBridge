import { Counter } from "./components/Counter";
import FetchData from "./components/FetchData";
import  Home  from "./components/Home";
import  CreateEvent  from "./components/Events/CreateEvent";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  },
  {
    path: '/create-event',
    element: <CreateEvent />
  }
];

export default AppRoutes;
