import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { updateDashboardRoom, getDashboardRoomUpdateById, createDashboardRoom } from '../../services/api_dashboard_room';
import RoomType from '../filter/FilterRoomTypes';

export default function RoomForm() {
  const { id } = useParams();
  const [mode, setMode] = useState('view');
  const [roomData, setRoomData] = useState({
    imageUrl: '',
    number: '',
    bedCount: 0,
    price: 0,
    typeId: '',
    isActive: true
  });

  useEffect(() => {
    if (id) {
      getDashboardRoomUpdateById(id)
        .then(response => {
          setRoomData(response.data);
        })
        .catch(error => console.error("Error fetching room details:", error));
    } else {
      setMode('add');
    }
  }, [id]);

  const handleFormSubmit = (e) => {
    e.preventDefault();
    if (mode === 'add') {
      createDashboardRoom(roomData)
        .then(() => handleSuccess())
        .catch(error => console.error("Error creating room:", error));
    } else if (mode === 'edit') {
      updateDashboardRoom(id, roomData)
        .then(() => handleSuccess())
        .catch(error => console.error("Error updating room:", error));
    }
  };

  const handleSuccess = () => {
    console.log('Room operation successful!');
  };

  const handleRoomTypeChange = (selectedRoomType) => {
    setRoomData({ 
      ...roomData, 
      typeId: selectedRoomType,
    });
  };

  const handleEdit = () => {
    setMode('edit'); 
  };

  return (
    <div>
      <br/>
      <form id="roomForm" onSubmit={handleFormSubmit}>
        <label htmlFor="imageUrl">Image url: </label><br />
        <input type="text" id="imageUrl" name="imageUrl" value={roomData.imageUrl}  disabled={mode === 'view'} onChange={(e) => setRoomData({ ...roomData, imageUrl: e.target.value })} /><br /><br />

        <label htmlFor="roomNumber">Room number: </label><br />
        <input type="text" id="roomNumber" name="roomNumber" value={roomData.number} disabled={mode === 'view'} onChange={(e) => setRoomData({ ...roomData, number: e.target.value })} /><br /><br />

        <label className="NumberOfBedsRow">
            <span>Number of beds:</span><br/>
            <input type="text" value={roomData.bedCount} disabled={mode === 'view'} onChange={(e) => setRoomData({ ...roomData, bedCount: e.target.value })} /><br/><br/>
        </label>

        <label htmlFor="price">Price for room: </label><br />
        <input type="number" id="price" name="price" value={roomData.price} disabled={mode === 'view'} onChange={(e) => setRoomData({ ...roomData, price: e.target.value })} /><br /><br />

        <RoomType onChangeHandle={handleRoomTypeChange} isDisabled={mode === 'view'} />

        <label>
        Is Active:
        <input 
          type="value"
          name="isActive" 
          value={roomData.isActive} 
          disabled={mode === 'view'} 
          onChange={(e) => setRoomData({ ...roomData, isActive: e.target.value })} 
        />
      </label>

        <br />
        {mode === 'view' && <button type="button" onClick={handleEdit}>Edit</button>} 
        {mode === 'edit' && <button type="submit">{mode === 'view' ? 'Edit' : 'Finish'}</button>}
        {mode === 'add' && <button type="submit">{mode === 'view' ? 'Edit' : 'Finish'}</button>}

      </form> 
    </div>
  );
}
