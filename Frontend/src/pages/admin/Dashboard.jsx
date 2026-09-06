import {
  Users,
  Building2,
  CalendarClock,
  Wallet,
  ArrowUpRight,
  ArrowDownRight,
  Clock,
  CheckCircle,
  XCircle,
  UserPlus,
} from "lucide-react";

const Dashboard = () => {
  const stats = [
    {
      title: "Total Employees",
      value: "124",
      change: "+8.2%",
      trend: "up",
      description: "from last month",
      icon: Users,
    },
    {
      title: "Departments",
      value: "8",
      change: "+1",
      trend: "up",
      description: "this month",
      icon: Building2,
    },
    {
      title: "Pending Leaves",
      value: "12",
      change: "-4.5%",
      trend: "down",
      description: "from last month",
      icon: CalendarClock,
    },
    {
      title: "Monthly Payroll",
      value: "$125,000",
      change: "+6.4%",
      trend: "up",
      description: "from last month",
      icon: Wallet,
    },
  ];

  const activities = [
    {
      icon: UserPlus,
      title: "New employee joined",
      description: "John Doe joined the Engineering department",
      time: "10 minutes ago",
    },
    {
      icon: CheckCircle,
      title: "Leave approved",
      description: "Sarah's leave request was approved",
      time: "1 hour ago",
    },
    {
      icon: Users,
      title: "Employee profile updated",
      description: "Michael updated his profile information",
      time: "3 hours ago",
    },
    {
      icon: Wallet,
      title: "Payroll processed",
      description: "August payroll has been processed",
      time: "Yesterday",
    },
  ];

  return (
    <div className="space-y-6">

      {/* Header */}

      <div>
        <h1 className="text-2xl font-bold text-gray-900">
          Dashboard
        </h1>

        <p className="mt-1 text-sm text-gray-500">
          Here's what's happening in your organization today.
        </p>
      </div>


      {/* Statistics */}

      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-4">

        {stats.map((stat) => {
          const Icon = stat.icon;

          return (
            <div
              key={stat.title}
              className="rounded-xl border border-gray-200 bg-white p-5 shadow-sm"
            >

              <div className="flex items-center justify-between">

                <div>
                  <p className="text-sm font-medium text-gray-500">
                    {stat.title}
                  </p>

                  <h2 className="mt-2 text-2xl font-bold text-gray-900">
                    {stat.value}
                  </h2>
                </div>

                <div className="rounded-lg bg-gray-100 p-3">
                  <Icon size={22} className="text-gray-700" />
                </div>

              </div>


              <div className="mt-4 flex items-center gap-2 text-sm">

                {stat.trend === "up" ? (
                  <ArrowUpRight
                    size={16}
                    className="text-green-600"
                  />
                ) : (
                  <ArrowDownRight
                    size={16}
                    className="text-red-600"
                  />
                )}

                <span
                  className={
                    stat.trend === "up"
                      ? "font-medium text-green-600"
                      : "font-medium text-red-600"
                  }
                >
                  {stat.change}
                </span>

                <span className="text-gray-400">
                  {stat.description}
                </span>

              </div>

            </div>
          );
        })}

      </div>


      {/* Charts section */}

      <div className="grid grid-cols-1 gap-6 xl:grid-cols-3">

        {/* Employee chart */}

        <div className="xl:col-span-2 rounded-xl border border-gray-200 bg-white p-6 shadow-sm">

          <div className="flex items-center justify-between">

            <div>
              <h2 className="font-semibold text-gray-900">
                Employee Statistics
              </h2>

              <p className="mt-1 text-sm text-gray-500">
                Employee growth over the last 6 months
              </p>
            </div>

            <select className="rounded-lg border border-gray-200 px-3 py-2 text-sm outline-none">
              <option>Last 6 months</option>
              <option>Last year</option>
            </select>

          </div>

          <div className="mt-6">
            <EmployeeChart />
          </div>

        </div>


        {/* Leave overview */}

        <div className="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">

          <h2 className="font-semibold text-gray-900">
            Leave Requests
          </h2>

          <p className="mt-1 text-sm text-gray-500">
            Current leave request status
          </p>


          <div className="mt-6 space-y-5">

            <LeaveStatus
              icon={Clock}
              label="Pending"
              value="12"
            />

            <LeaveStatus
              icon={CheckCircle}
              label="Approved"
              value="45"
            />

            <LeaveStatus
              icon={XCircle}
              label="Rejected"
              value="5"
            />

          </div>

        </div>

      </div>


      {/* Bottom section */}

      <div className="grid grid-cols-1 gap-6 xl:grid-cols-2">

        {/* Recent activities */}

        <div className="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">

          <div className="flex items-center justify-between">

            <h2 className="font-semibold text-gray-900">
              Recent Activities
            </h2>

            <button className="text-sm font-medium text-blue-600 hover:text-blue-700">
              View all
            </button>

          </div>


          <div className="mt-6 space-y-5">

            {activities.map((activity, index) => {

              const Icon = activity.icon;

              return (
                <div
                  key={index}
                  className="flex gap-4"
                >

                  <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-gray-100">
                    <Icon
                      size={18}
                      className="text-gray-600"
                    />
                  </div>

                  <div className="min-w-0">

                    <p className="text-sm font-medium text-gray-900">
                      {activity.title}
                    </p>

                    <p className="mt-1 text-sm text-gray-500">
                      {activity.description}
                    </p>

                    <p className="mt-1 flex items-center gap-1 text-xs text-gray-400">
                      <Clock size={12} />
                      {activity.time}
                    </p>

                  </div>

                </div>
              );

            })}

          </div>

        </div>


        {/* Payroll */}

        <div className="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">

          <div className="flex items-center justify-between">

            <div>
              <h2 className="font-semibold text-gray-900">
                Payroll Summary
              </h2>

              <p className="mt-1 text-sm text-gray-500">
                Current month's payroll
              </p>
            </div>

            <Wallet
              size={22}
              className="text-gray-600"
            />

          </div>


          <div className="mt-6">

            <p className="text-sm text-gray-500">
              Total Payroll
            </p>

            <h3 className="mt-1 text-3xl font-bold text-gray-900">
              $125,000
            </h3>

          </div>


          <div className="mt-6 grid grid-cols-2 gap-4">

            <div className="rounded-lg bg-gray-50 p-4">

              <p className="text-sm text-gray-500">
                Processed
              </p>

              <p className="mt-1 text-xl font-semibold text-gray-900">
                $115,000
              </p>

            </div>


            <div className="rounded-lg bg-gray-50 p-4">

              <p className="text-sm text-gray-500">
                Pending
              </p>

              <p className="mt-1 text-xl font-semibold text-gray-900">
                $10,000
              </p>

            </div>

          </div>

        </div>

      </div>

    </div>
  );
};


const LeaveStatus = ({ icon: Icon, label, value }) => {
  return (
    <div className="flex items-center justify-between">

      <div className="flex items-center gap-3">

        <div className="rounded-lg bg-gray-100 p-2">
          <Icon size={18} className="text-gray-600" />
        </div>

        <span className="text-sm font-medium text-gray-700">
          {label}
        </span>

      </div>

      <span className="text-lg font-semibold text-gray-900">
        {value}
      </span>

    </div>
  );
};


const EmployeeChart = () => {
  const data = [
    { month: "Apr", employees: 98 },
    { month: "May", employees: 105 },
    { month: "Jun", employees: 111 },
    { month: "Jul", employees: 116 },
    { month: "Aug", employees: 120 },
    { month: "Sep", employees: 124 },
  ];

  const max = Math.max(...data.map((item) => item.employees));

  return (
    <div className="flex h-64 items-end gap-4">

      {data.map((item) => {

        const height = (item.employees / max) * 100;

        return (
          <div
            key={item.month}
            className="flex flex-1 flex-col items-center gap-2"
          >

            <div className="flex h-full w-full items-end">

              <div
                className="w-full rounded-t-lg bg-blue-500"
                style={{
                  height: `${height}%`,
                }}
              />

            </div>

            <span className="text-xs text-gray-500">
              {item.month}
            </span>

          </div>
        );

      })}

    </div>
  );
};


export default Dashboard;