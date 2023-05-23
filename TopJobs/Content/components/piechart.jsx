import React, { Fragment } from 'react';
import { LineChart, Line } from 'recharts';
const data = [{ name: 'Page A', uv: 400, pv: 2400, amt: 2400 }, { name: 'Page B', uv: 200, pv: 2000, amt: 2600 }];

export function PieChartDemo() {
	return (
		<Fragment>
			<LineChart width={400} height={400} data={data}>
				<Line type="monotone" dataKey="uv" stroke="#8884d8" />
			</LineChart>
		</Fragment>
	);
}
