import axios from "axios";
import React, { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { API_BASE_URL } from '../api';

export default function ViewUser() {


    const [user, setUser] = useState({
        name: "",
        username: "",
        email: ""

    })

    const { id } = useParams();

    useEffect(() => {
        loadUser()
    }, [])

    const loadUser = async () => {
        const result = await axios.get(`${API_BASE_URL}/user/${id}`);
        setUser(result.data)
    }



    return (
        <div className="container">
            <div className="row">
                <div className="col-md-6 offset-md-3 border rounded p-4 mt-2 shadow">
                    <h2 className="text-center m-4">User Details</h2>


                    <div className="card">
                        <div classname="card-header">
                            Details of user id : {user.id}
                            <ul classname="list-group list-group-flush">
                                <li className="list-group-item">
                                    <b>Name:</b>
                                    {user.name}

                                </li>
                                <li className="list-group-item">
                                    <b>UserName:</b>
                                    {user.username}

                                </li>
                                <li className="list-group-item">
                                    <b>Email:</b>
                                    {user.email}

                                </li>
                            </ul>
                        </div>
                    </div>
                    <Link className="btn btn-primary mx-2" to={"/"}>Back to Home</Link>
                </div>
            </div>
        </div>
    )




}