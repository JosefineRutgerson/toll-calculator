import Header from "./Header"
import Footer from "./Footer"
import { Outlet } from "react-router-dom"

export default function Layout() {
  return (
    <div className="myapp">
      <Header />
      <main style={{ padding: "1rem" }}>
        <Outlet />
      </main>
      <Footer />
    </div>
  )
}