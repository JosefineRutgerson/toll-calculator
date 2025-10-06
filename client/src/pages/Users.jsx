import { useEffect, useState } from "react"
import { Link } from "react-router-dom";

export default function Users() {
  const [users, setUsers] = useState([])

  useEffect(() => {
    fetch("/api/users")
      .then(res => res.json())
      .then(data => setUsers(data))
      .catch(err => console.error(err))
  }, [])

  return (
    <div>
      <h1>Användare</h1>
      <ul>
        {users.map(user => (
          <li key={user.id}>
            <Link to={`/users/${encodeURIComponent(user.name)}`}>
                {user.name}
            </Link>
          </li>
        ))}
      </ul>
    </div>
  )
}
