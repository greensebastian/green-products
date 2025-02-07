import { NavLink } from "react-router";

function NavigationBar() {
  return (
    <nav className="bg-gray-800 p-4 flex space-x-4">
      <NavLink
        to="/"
        className={({ isActive }) =>
          `text-white px-3 py-2 rounded-md text-sm font-medium ${
            isActive ? "bg-gray-900" : "hover:bg-gray-700"
          }`
        }
        end
      >
        Products
      </NavLink>
      <NavLink
        to="/create-product"
        className={({ isActive }) =>
          `text-white px-3 py-2 rounded-md text-sm font-medium ${
            isActive ? "bg-gray-900" : "hover:bg-gray-700"
          }`
        }
        end
      >
        Create Product
      </NavLink>
    </nav>
  );
}

export default NavigationBar;
